using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.AI
{
    public interface I_GoapActionSet
    {
        I_GoapAction[] GetActions();
    }

    public interface I_GoapAction
    {
        HashSet<string> GetPreConditions();
        HashSet<string> GetPostEffects();
        bool IsProceduralConditionValid(Agent _agent);
    }

    public class GoapActionConfig : ScriptableObject, I_GoapAction, ISerializationCallbackReceiver
    {
        HashSet<string> preConditions = new HashSet<string>();
        HashSet<string> postEffects = new HashSet<string>();

        public HashSet<string> GetPreConditions()
        {
            return preConditions;
        }
        public HashSet<string> GetPostEffects()
        {
            return postEffects;
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            //create the hashsets
        }

        public virtual bool IsProceduralConditionValid(Agent _agent)
        {
            return true;
        }
    }
}
