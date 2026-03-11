#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class EmojiDatabaseBuilder
{
    private const string EmojiTestUrl = "https://unicode.org/Public/emoji/latest/emoji-test.txt";
    private const float MaxEmojiVersion = 12.0f;

    public static void BuildEmojiDatabase()
    {
        try
        {
            EditorUtility.DisplayProgressBar("Emoji Database", "Downloading from unicode.org ...", 0.1f);
            string dataAssetPath = LogEmojiPickerPopup.GetDataAssetPath();

            string raw;
            using (var wc = new WebClient { Encoding = Encoding.UTF8 })
                raw = wc.DownloadString(EmojiTestUrl);

            EditorUtility.DisplayProgressBar("Emoji Database", "Parsing ...", 0.5f);
            var entries = ParseEmojiTest(raw);

            EditorUtility.DisplayProgressBar("Emoji Database", "Writing JSON ...", 0.85f);
            WriteJson(entries, dataAssetPath);

            AssetDatabase.Refresh();
            LogEmojiPickerPopup.InvalidateCache();
        }
        catch (Exception ex)
        {
            Debug.LogError($"EmojiDatabase- Build failed – {ex.Message}\n{ex.StackTrace}");
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    private static readonly Dictionary<string, int> GroupMap =
        new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            ["Smileys & Emotion"] = 0,  
            ["People & Body"]     = 0,
            ["Animals & Nature"]  = 1,
            ["Food & Drink"]      = 2, 
            ["Activities"]        = 3,  
            ["Travel & Places"]   = 4,  
            ["Objects"]           = 5,  
            ["Symbols"]           = 6,  
            ["Flags"]             = 7, 
        };

    private struct ParsedEmoji { public string v, n; public int c; }

    private static List<ParsedEmoji> ParseEmojiTest(string text)
    {
        var result   = new List<ParsedEmoji>(2000);
        int category = -1;

        var versionRe = new Regex(@"E(\d+\.\d+) ", RegexOptions.Compiled);

        foreach (string rawLine in text.Split('\n'))
        {
            string line = rawLine.Trim();

            if (line.StartsWith("# group:"))
            {
                string g = line.Substring(8).Trim();
                category = GroupMap.TryGetValue(g, out int c) ? c : -1;
                continue;
            }

            if (category < 0 || line.Length == 0 || line[0] == '#') continue;
            if (!line.Contains("; fully-qualified")) continue;

            int semi = line.IndexOf(';');
            if (semi < 0) continue;
            string   cpField  = line.Substring(0, semi).Trim();
            string[] cpTokens = cpField.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var  cps   = new int[cpTokens.Length];
            bool valid = true;
            for (int i = 0; i < cpTokens.Length; i++)
            {
                try   { cps[i] = Convert.ToInt32(cpTokens[i], 16); }
                catch { valid  = false; break; }
            }
            if (!valid) continue;

            if (Array.IndexOf(cps, 0x200D) >= 0) continue;
            if (HasExcludedCodePointRange(cps)) continue;

            bool hasSkinTone = false;
            foreach (int cp in cps) if (cp >= 0x1F3FB && cp <= 0x1F3FF) { hasSkinTone = true; break; }
            if (hasSkinTone) continue;

            bool hasHairMod = false;
            foreach (int cp in cps) if (cp >= 0x1F9B0 && cp <= 0x1F9B3) { hasHairMod = true; break; }
            if (hasHairMod) continue;

            int hash = line.IndexOf('#');
            if (hash < 0) continue;
            string comment = line.Substring(hash + 1).Trim();

            var m = versionRe.Match(comment);
            if (!m.Success) continue;

            if (float.TryParse(m.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float ver)
                && ver > MaxEmojiVersion)
                continue;

            string emoji = comment.Substring(0, m.Index).Trim();
            string name  = comment.Substring(m.Index + m.Length).Trim(); 

            if (emoji.Length > 0 && name.Length > 0)
                result.Add(new ParsedEmoji { v = emoji, n = name, c = category });
        }

        return result;
    }

    private static bool HasExcludedCodePointRange(int[] cps)
    {
        foreach (int cp in cps)
        {
            // Extended-A block often contains glyphs that fallback-render as identical symbols in IMGUI.
            if (cp >= 0x1FA70 && cp <= 0x1FAFF)
                return true;
        }

        return false;
    }

    private static void WriteJson(List<ParsedEmoji> emojis, string dataAssetPath)
    {
        var sb = new StringBuilder(emojis.Count * 60);
        sb.Append("{\"emojis\":[");

        for (int i = 0; i < emojis.Count; i++)
        {
            if (i > 0) sb.Append(',');
            string escaped = emojis[i].n
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"");
            sb.Append("{\"v\":\"")
              .Append(emojis[i].v)
              .Append("\",\"n\":\"")
              .Append(escaped)
              .Append("\",\"c\":")
              .Append(emojis[i].c)
              .Append('}');
        }

        sb.Append("]}");

        string fullPath = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory(), dataAssetPath));

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);
    }
}

#endif
