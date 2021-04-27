#region Description
// StatPropertyDrawer.cs
// 04-24-2021
// James LaFritz
#endregion

using UnityEditor;
using UnityEngine;

//[CustomPropertyDrawer(typeof(Stat))]
public class StatPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        // Get properties
        SerializedProperty maxValue = property?.FindPropertyRelative("maxValue");

        // Store old indent level and set it to 0, the PrefixLabel takes care of it
        int indent = EditorGUI.indentLevel;
        //EditorGUI.indentLevel = 0;

        EditorGUI.PropertyField(position, maxValue);

        if (EditorGUI.EndChangeCheck())
            property?.serializedObject?.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}