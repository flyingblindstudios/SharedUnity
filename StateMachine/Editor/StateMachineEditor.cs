﻿
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class StateMachineEditor : EditorWindow
{

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/StateMachineEditor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(StateMachineEditor));
    }

    void OnGUI()
    {

        /*Agent a = Selection.activeGameObject.GetComponent<Agent>();

        if (a == null)
        {
            return;
        }

        StateMachine st = a.GetStateMachine();
        st.*/


        /*GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();*/
    }
}
#endif