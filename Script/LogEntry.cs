namespace UnityEngine
{
    public readonly struct LogEntry
    {
        public readonly LogType Level;
        public readonly Channel Channel;
        public readonly object Message;
        public readonly string OverrideColor;

        public LogEntry(LogType level, Channel channel, object message, string overrideColor = null)
        {
            Level = level;
            Channel = channel;
            Message = message;
            OverrideColor = overrideColor;
        }

        public static LogEntry Create(LogType level, object message, string overrideColor = null)
        {
            return new LogEntry(level, Channel.Default, message, overrideColor);
        }

        public static LogEntry Create(LogType level, Channel channel, object message, string overrideColor = null)
        {
            return new LogEntry(level, channel, message, overrideColor);
        }
    }
}