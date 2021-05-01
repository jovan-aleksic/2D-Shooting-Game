using UnityEditor;
using UnityEngine;

public enum InfoBoxType
{
    Normal,
    Warning,
    Error
}

[CustomPropertyDrawer(typeof(InfoBoxAttribute))]
public class InfoBoxAttribute : PropertyAttribute
{
    public readonly string text;
    public readonly InfoBoxType infoBoxType;

    public InfoBoxAttribute(string text, InfoBoxType type = InfoBoxType.Normal)
    {
        this.text = text;
        infoBoxType = type;
    }
}
