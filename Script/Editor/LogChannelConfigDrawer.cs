#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LogChannelConfig))]
public class LogChannelConfigDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        const int lineCount = 5;
        return lineCount * EditorGUIUtility.singleLineHeight + (lineCount - 1) * EditorGUIUtility.standardVerticalSpacing;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect lineRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(lineRect, property.isExpanded, label, true);

        if (property.isExpanded)
        {
            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            var channelNameProp = property.FindPropertyRelative("channelName");
            var emojiProp = property.FindPropertyRelative("emoji");
            var colorProp = property.FindPropertyRelative("channelColor");
            var enabledProp = property.FindPropertyRelative("enabled");

            EditorGUI.PropertyField(lineRect, channelNameProp);

            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            DrawEmojiField(lineRect, emojiProp);

            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(lineRect, colorProp);

            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(lineRect, enabledProp);
        }

        EditorGUI.EndProperty();
    }

    private static void DrawEmojiField(Rect rect, SerializedProperty emojiProp)
    {
        const float buttonWidth = 70f;
        Rect textRect = new Rect(rect.x, rect.y, rect.width - buttonWidth - 6f, rect.height);
        Rect buttonRect = new Rect(textRect.xMax + 6f, rect.y, buttonWidth, rect.height);

        string buttonText = string.IsNullOrEmpty(emojiProp.stringValue) ? "선택" : emojiProp.stringValue;
        EditorGUI.LabelField(textRect, "Emoji", buttonText);

        if (GUI.Button(buttonRect, "선택"))
            LogEmojiPickerPopup.Show(buttonRect, emojiProp.serializedObject, emojiProp.propertyPath);
    }
}

#endif
