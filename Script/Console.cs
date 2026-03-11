using System.Diagnostics;

namespace UnityEngine
{
    public static class Console
    {
        private const string ResourcePath = "LogChannelAsset";

        private static LogChannelAsset _asset;
        private static bool _initialized;

        private static LogChannelAsset Asset
        {
            get
            {
                if (!_initialized)
                {
                    _asset = Resources.Load<LogChannelAsset>(ResourcePath);
                    _initialized = true;

                    if (_asset == null) { Debug.LogWarning($"<color=#FF8888>Console> LogChannelAsset 이 존재 하지않음 '{ResourcePath}</color>'"); }
                }
                return _asset;
            }
        }

        #region log
        [Conditional("ENABLE_GAME_LOG")]
        public static void Log(object message)
            => Output(LogEntry.Create(LogType.Log, message, null));

        [Conditional("ENABLE_GAME_LOG")]
        public static void Log(object message, string color)
            => Output(LogEntry.Create(LogType.Log, message, color));

        [Conditional("ENABLE_GAME_LOG")]
        public static void Log(Channel channel, object message)
            => Output(LogEntry.Create(LogType.Log, channel, message, null));

        [Conditional("ENABLE_GAME_LOG")]
        public static void Log(Channel channel, string message)
            => Output(LogEntry.Create(LogType.Log, channel, message, null));

        [Conditional("ENABLE_GAME_LOG")]
        public static void Log(Channel channel, object message, string color)
            => Output(LogEntry.Create(LogType.Log, channel, message, color));

        #endregion

        #region log warning
        [Conditional("ENABLE_GAME_LOG")]
        public static void LogWarning(object message)
            => Output(LogEntry.Create(LogType.Warning, message, null));

        [Conditional("ENABLE_GAME_LOG")]
        public static void LogWarning(object message, string color)
            => Output(LogEntry.Create(LogType.Warning, message, color));

        [Conditional("ENABLE_GAME_LOG")]
        public static void LogWarning(Channel channel, object message)
            => Output(LogEntry.Create(LogType.Warning, channel, message, null));

        [Conditional("ENABLE_GAME_LOG")]
        public static void LogWarning(Channel channel, string message)
            => Output(LogEntry.Create(LogType.Warning, channel, message, null));

        [Conditional("ENABLE_GAME_LOG")]
        public static void LogWarning(Channel channel, object message, string color)
            => Output(LogEntry.Create(LogType.Warning, channel, message, color));

        #endregion

        #region log error
        [Conditional("ENABLE_GAME_LOG")]
        public static void LogError(object message)
            => Output(LogEntry.Create(LogType.Error, message, null));

        [Conditional("ENABLE_GAME_LOG")]
        public static void LogError(object message, string color)
            => Output(LogEntry.Create(LogType.Error, message, color));

        [Conditional("ENABLE_GAME_LOG")]
        public static void LogError(Channel channel, object message)
            => Output(LogEntry.Create(LogType.Error, channel, message, null));

        [Conditional("ENABLE_GAME_LOG")]
        public static void LogError(Channel channel, string message)
            => Output(LogEntry.Create(LogType.Error, channel, message, null));

        [Conditional("ENABLE_GAME_LOG")]
        public static void LogError(Channel channel, object message, string color)
            => Output(LogEntry.Create(LogType.Error, channel, message, color));

        #endregion

        #region log exception
        public static void LogException(System.Exception exception)
    => Debug.LogException(exception);

        #endregion

        private static void Output(LogEntry entry)
        {
            LogChannelConfig config = null;

            if (Asset != null)
            {
                Asset.TryGetChannel((int)entry.Channel, out config);

                if (config is { enabled: false })
                    return;
            }

            string formatted = LogFormatter.Format(entry, config, Asset);

            switch (entry.Level)
            {
                case LogType.Log: Debug.Log(formatted); break;
                case LogType.Warning: Debug.LogWarning(formatted); break;
                case LogType.Error: Debug.LogError(formatted); break;
            }
        }
    }
}