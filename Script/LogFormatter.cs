using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine
{
    public static class LogFormatter
    {
        private const string DefaultPrefix = "…";
        private const string DefaultSuffix = "≫";

        private static StringBuilder stringBuilder => _stringBuilder ??= new StringBuilder(256);

        [ThreadStatic]
        private static StringBuilder _stringBuilder;
        private static readonly Dictionary<string, string> ColorHexCache = new();

        #region public
        public static string Format(LogEntry entry, LogChannelConfig channelConfig, LogChannelAsset asset)
        {
            stringBuilder.Clear();

            AppendChannelPrefix(entry, channelConfig, asset);
            AppendMessage(entry);

            return stringBuilder.ToString();
        }

        #endregion

        #region private
        private static void AppendChannelPrefix(LogEntry entry, LogChannelConfig config, LogChannelAsset asset)
        {
            string prefix = asset != null ? asset.Prefix : DefaultPrefix;
            string suffix = asset != null ? asset.Suffix : DefaultSuffix;

            if (config != null)
            {
                string hex = GetOrCacheHex(config);
                string displayName = GetDisplayName(config);
                stringBuilder.Append("<color=#").Append(hex).Append('>')
                                .Append(prefix)
                                .Append(displayName)
                                .Append(suffix)
                                .Append("</color> ");
            }
            else
            {
                stringBuilder.Append(prefix)
                               .Append(entry.Channel.ToString())
                               .Append(suffix)
                               .Append(' ');
            }
        }

        private static void AppendMessage(LogEntry entry)
        {
            string messageText = entry.Message?.ToString() ?? "null";

            if (!string.IsNullOrEmpty(entry.OverrideColor))
            {
                int start = entry.OverrideColor[0] == '#' ? 1 : 0;
                stringBuilder.Append("<color=#")
                                .Append(entry.OverrideColor, start, entry.OverrideColor.Length - start)
                                .Append('>')
                                .Append(messageText)
                                .Append("</color>");
            }
            else
            {
                stringBuilder.Append(messageText);
            }
        }

        private static string GetOrCacheHex(LogChannelConfig config)
        {
            if (!ColorHexCache.TryGetValue(config.channelName, out string hex))
            {
                hex = ColorUtility.ToHtmlStringRGB(config.channelColor);
                ColorHexCache[config.channelName] = hex;
            }
            return hex;
        }

        private static string GetDisplayName(LogChannelConfig config)
        {
            if (!string.IsNullOrWhiteSpace(config.emoji))
                return config.emoji + " " + config.channelName;

            return config.channelName;
        }

        #endregion

        public static void InvalidateHexCache() => ColorHexCache.Clear();
    }
}