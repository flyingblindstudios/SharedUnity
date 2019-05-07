using System;
using UnityEngine;

namespace Shared.Data
{
    [CreateAssetMenu(menuName = "Shared/Data/Vector3Variable")]
    public class Vector3Variable : VariableReference<Vector3>, ISerializationCallbackReceiver
    {
     
    }
}