using System;
using UnityEngine;

namespace Shared.Data
{
    [CreateAssetMenu(menuName = "Shared/Data/FloatVariable")]
    public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        public Shared.Event.GameEvent OnChange = null;

        [SerializeField]
        private float InitialValue;

        [NonSerialized]
        public float RuntimeValue;


        public void SetValue( float _value )
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