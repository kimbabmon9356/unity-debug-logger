#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LogEmojiPickerPopup : PopupWindowContent
{
    private const string DataFileName = "emoji-data.json";

    private readonly SerializedObject _serializedObject;
    private readonly string           _propertyPath;

    private Vector2 _scroll;
    private string  _search = string.Empty;
    private int     _selectedCategory;
    private bool    _focusedSearch;

    private static List<EmojiEntry> _cache;

    public static void InvalidateCache() => _cache = null;

    internal static string GetDataAssetPath()
    {
        string[] guids = AssetDatabase.FindAssets("LogEmojiPickerPopup t:Script");
        if (guids != null && guids.Length > 0)
        {
            string scriptPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            string dir = Path.GetDirectoryName(scriptPath);
            if (!string.IsNullOrEmpty(dir))
                return dir.Replace('\\', '/') + "/" + DataFileName;
        }

        return "Assets/" + DataFileName;
    }

    private static List<EmojiEntry> GetAllEmojis()
    {
        if (_cache != null) return _cache;

        var ta = AssetDatabase.LoadAssetAtPath<TextAsset>(GetDataAssetPath());
        if (ta == null) return null;

        var data = JsonUtility.FromJson<EmojiJsonData>(ta.text);
        if (data?.emojis == null) return null;

        _cache = new List<EmojiEntry>(data.emojis.Length);
        foreach (var e in data.emojis)
        {
            if (string.IsNullOrEmpty(e.v) || IsExcludedEmojiValue(e.v))
                continue;

            _cache.Add(new EmojiEntry(e.v, e.n, (EmojiCategory)e.c));
        }

        return _cache;
    }

    private static readonly GUIContent[] CategoryTabs =
    {
        new GUIContent("😀", "Smileys & People"),
        new GUIContent("🐻", "Animal & Nature"),
        new GUIContent("🍔", "Food & Drink"),
        new GUIContent("⚽", "Activity"),
        new GUIContent("✈️", "Travel & Places"),
        new GUIContent("💡", "Objects"),
        new GUIContent("♻️", "Symbols"),
        new GUIContent("🏁", "Flags"),
    };

    public static void Show(Rect activatorRect, SerializedObject so, string propertyPath)
        => PopupWindow.Show(activatorRect, new LogEmojiPickerPopup(so, propertyPath));

    private LogEmojiPickerPopup(SerializedObject so, string propertyPath)
    {
        _serializedObject = so;
        _propertyPath     = propertyPath;
    }

    public override Vector2 GetWindowSize() => new Vector2(360f, 430f);

    public override void OnGUI(Rect rect)
    {
        var emojis = GetAllEmojis();
        if (emojis == null)
        {
            DrawNoDatabaseUI();
            return;
        }

        DrawCategoryTabs();
        DrawSearch();

        _scroll = GUILayout.BeginScrollView(_scroll);
        DrawSectionTitle(GetCategoryTitle((EmojiCategory)_selectedCategory));
        DrawEmojiGrid(Filter(_search, _selectedCategory, emojis));
        GUILayout.EndScrollView();
    }

    private void DrawNoDatabaseUI()
    {
        GUILayout.Space(20f);
        EditorGUILayout.HelpBox(
            "이모지 데이터베이스가 없습니다. 아래 버튼을 눌러 데이터베이스를 빌드하세요.",
            MessageType.Info);
        GUILayout.Space(10f);
        if (GUILayout.Button("Build Emoji Database", GUILayout.Height(32f)))
        {
            EmojiDatabaseBuilder.BuildEmojiDatabase();
            editorWindow.Repaint();
        }
    }

    private void DrawCategoryTabs()
    {
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal();
        GUILayout.Space(6f);
        _selectedCategory = GUILayout.Toolbar(_selectedCategory, CategoryTabs, GUILayout.Height(26f));
        GUILayout.Space(6f);
        GUILayout.EndHorizontal();
    }

    private void DrawSearch()
    {
        GUILayout.Space(6f);
        GUILayout.BeginHorizontal();
        GUILayout.Space(6f);
        GUI.SetNextControlName("EmojiSearch");
        _search = EditorGUILayout.TextField(_search, EditorStyles.toolbarSearchField);
        GUILayout.Space(6f);
        GUILayout.EndHorizontal();
        GUILayout.Space(4f);

        if (!_focusedSearch && Event.current.type == EventType.Repaint)
        {
            EditorGUI.FocusTextInControl("EmojiSearch");
            _focusedSearch = true;
        }
    }

    private static void DrawSectionTitle(string title)
    {
        GUILayout.Label(title, EditorStyles.boldLabel);
        GUILayout.Space(2f);
    }

    private void DrawEmojiGrid(List<EmojiEntry> emojis)
    {
        if (emojis.Count == 0)
        {
            EditorGUILayout.HelpBox("검색 결과가 없습니다.", MessageType.Info);
            return;
        }

        const int cols = 8;
        int rows = Mathf.CeilToInt(emojis.Count / (float)cols);

        for (int r = 0; r < rows; r++)
        {
            GUILayout.BeginHorizontal();
            for (int c = 0; c < cols; c++)
            {
                int i = r * cols + c;
                if (i >= emojis.Count) { GUILayout.Space(36f); continue; }
                DrawEmojiButton(emojis[i]);
            }
            GUILayout.EndHorizontal();
        }
    }

    private void DrawEmojiButton(EmojiEntry entry)
    {
        if (!GUILayout.Button(
            new GUIContent(entry.Value, entry.Name),
            GUILayout.Width(36f), GUILayout.Height(36f))) return;

        var prop = _serializedObject.FindProperty(_propertyPath);
        if (prop == null) return;

        prop.stringValue = entry.Value;
        _serializedObject.ApplyModifiedProperties();
        editorWindow.Close();
    }

    private static List<EmojiEntry> Filter(string search, int catIndex, List<EmojiEntry> all)
    {
        var    result   = new List<EmojiEntry>();
        var    category = (EmojiCategory)catIndex;
        string kw       = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

        foreach (var e in all)
        {
            if (e.Category != category) continue;
            if (kw == null || e.Name.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                result.Add(e);
        }

        return result;
    }

    private static bool IsExcludedEmojiValue(string value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            int cp = char.ConvertToUtf32(value, i);
            if (char.IsHighSurrogate(value[i]))
                i++;

            // Extended-A block often appears as duplicated/fallback glyphs in some editor environments.
            if (cp >= 0x1FA70 && cp <= 0x1FAFF)
                return true;
        }

        return false;
    }

    private static string GetCategoryTitle(EmojiCategory cat)
    {
        switch (cat)
        {
            case EmojiCategory.SmileysAndPeople: return "Smileys & People";
            case EmojiCategory.AnimalAndNature:  return "Animal & Nature";
            case EmojiCategory.FoodAndDrink:     return "Food & Drink";
            case EmojiCategory.Activity:         return "Activity";
            case EmojiCategory.TravelAndPlaces:  return "Travel & Places";
            case EmojiCategory.Objects:          return "Objects";
            case EmojiCategory.Symbols:          return "Symbols";
            case EmojiCategory.Flags:            return "Flags";
            default:                             return "Emojis";
        }
    }

    private readonly struct EmojiEntry
    {
        public readonly string       Value, Name;
        public readonly EmojiCategory Category;
        public EmojiEntry(string v, string n, EmojiCategory c) { Value = v; Name = n; Category = c; }
    }

    private enum EmojiCategory
    {
        SmileysAndPeople = 0,
        AnimalAndNature  = 1,
        FoodAndDrink     = 2,
        Activity         = 3,
        TravelAndPlaces  = 4,
        Objects          = 5,
        Symbols          = 6,
        Flags            = 7,
    }

    [Serializable] private class EmojiJsonData  { public EmojiJsonEntry[] emojis; }
    [Serializable] private class EmojiJsonEntry { public string v, n; public int c; }
}

#endif
