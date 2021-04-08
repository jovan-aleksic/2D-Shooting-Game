#region Description

// 03-05-2021
// James LaFritz
// ----------------------------------------------------------------------------
// Based on
//
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

#endregion

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StringReference))]
public class StringReferenceDrawer : PropertyDrawer
{
    /// <summary>
    /// Options to display in the popup to select constant or variable.
    /// </summary>
    private readonly string[] m_popupOptions =
        {"Use Constant", "Use Variable"};

    /// <summary> Cached style to use to draw the popup button. </summary>
    private GUIStyle m_popupStyle;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        VariableReferencePropertyDrawer.OnGUI(position, property, label);
    }
}