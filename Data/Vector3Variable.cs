using System;
using UnityEngine;

namespace Shared.Data
{
    [CreateAssetMenu(menuName = "Shared/Data/Vector3Variable")]
    public class Vector3Variable : ScriptableObject, ISerializationCallbackReceiver
    {
        public Shared.Event.GameEvent OnChange = null;

        [SerializeField]
        private Vector3 InitialValue;

        [NonSerialized]
        public Vector3 RuntimeValue;


        public void SetValue( Vector3 _value )
        {
            RuntimeValue = _value;

            if (OnChange != null)
            {
                OnChange.Raise();
            }
        }

        public void OnAfterDeserialize()
        {
            RuntimeValue = InitialValue;
        }

        public void OnBeforeSerialize() { }
    }
}