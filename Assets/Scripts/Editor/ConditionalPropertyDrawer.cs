using UnityEditor;
using UnityEngine;

/// <summary>
/// Based on: https://forum.unity.com/threads/draw-a-field-only-if-a-condition-is-met.448855/
/// </summary>
[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldPropertyDrawer : PropertyDrawer
{
    #region Fields

    // Reference to the attribute on the property.
    ConditionalFieldAttribute ConditionalField;

    // Field that is being compared.
    SerializedProperty comparedField;

    #endregion

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!ShowMe(property) && ConditionalField.disablingType == ConditionalFieldAttribute.DisablingType.DontDraw)
            return 0f;

        // The height of the property should be defaulted to the default height.
        return base.GetPropertyHeight(property, label);
    }

    /// <summary>
    /// Errors default to showing the property.
    /// </summary>
    private bool ShowMe(SerializedProperty property)
    {
        ConditionalField = attribute as ConditionalFieldAttribute;
        // Replace propertyname to the value from the parameter
        string path = property.propertyPath.Contains(".") ? System.IO.Path.ChangeExtension(property.propertyPath, ConditionalField.comparedPropertyName) : ConditionalField.comparedPropertyName;

        comparedField = property.serializedObject.FindProperty(path);

        if (comparedField == null)
        {
            Debug.LogError("Cannot find property with name: " + path);
            return true;
        }

        // get the value & compare based on types
        switch (comparedField.type)
        { // Possible extend cases to support your own type
            case "bool":
                bool boolResult = comparedField.boolValue.Equals(ConditionalField.comparedValue);
                if (ConditionalField.comparingType == ConditionalFieldAttribute.ComparingType.NotEqual)
                {
                    boolResult = !boolResult;
                }
                return boolResult;
            case "Enum":
                bool enumResult = comparedField.enumValueIndex.Equals((int)ConditionalField.comparedValue);
                if (ConditionalField.comparingType == ConditionalFieldAttribute.ComparingType.NotEqual)
                {
                    enumResult = !enumResult;
                }
                return enumResult;
            default:
                Debug.LogError("Error: " + comparedField.type + " is not supported of " + path);
                return true;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // If the condition is met, simply draw the field.
        if (ShowMe(property))
        {
            EditorGUI.PropertyField(position, property, label);
        } //...check if the disabling type is read only. If it is, draw it disabled
        else if (ConditionalField.disablingType == ConditionalFieldAttribute.DisablingType.ReadOnly)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }

}