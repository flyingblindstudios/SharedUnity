using System;
using UnityEngine;

namespace Shared.Data
{
    [CreateAssetMenu(menuName = "Shared/Data/BoolVariable")]
    public class BoolVariable : VariableReference<bool>, ISerializationCallbackReceiver
    {
     
    }
}