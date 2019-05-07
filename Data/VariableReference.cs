using System;
using UnityEngine;
namespace Shared.Data
{
    public class VariableReference<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        public Shared.Event.GameEvent OnChange = null;

        [SerializeField]
        private T InitialValue;

        //[NonSerialized]
        public T RuntimeValue;


        public void SetValue(T _value)
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