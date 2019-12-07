#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Shared.AI
{ 
    [CustomPropertyDrawer(typeof(GoapID))]
    public class GoapIDDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int currentValue = property.FindPropertyRelative("value").intValue;

            int newValue = EditorGUI.Popup(position, currentValue, GoapID.LayerNames.ToArray());

            property.FindPropertyRelative("value").intValue = newValue;


            EditorGUI.EndProperty();
        }
    }
}
#endif