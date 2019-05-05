#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Shared.Event
{ 
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameEvent eventScript = (GameEvent) target;
            if (GUILayout.Button("Raise"))
            {
                eventScript.Raise();
            }
        }
    }
}

#endif
