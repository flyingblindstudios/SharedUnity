#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Shared.AI
{ 
    [CustomPropertyDrawer(typeof(GoapEffect))]
    public class GoapEffectDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);
            //position.width = position.width * 0.85f;
            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int currentValue = property.FindPropertyRelative("value").intValue;

            int newValue = EditorGUI.Popup(position, currentValue, GoapEffect.IdNames.ToArray());

            property.FindPropertyRelative("value").intValue = newValue;

            /*bool stateValue = property.FindPropertyRelative("state").boolValue;

            bool newStateValue = EditorGUI.Toggle(position, stateValue);

            property.FindPropertyRelative("state").boolValue = newStateValue;
            EditorGUI.EndProperty();*/


        }
    }
}
#endif