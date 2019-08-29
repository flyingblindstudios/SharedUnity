using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.AI
{
    public interface I_GoapActionSet
    {
        I_GoapAction[] GetActions();
    }

    public interface I_GoapAction : ICloneable
    {
        HashSet<string> GetPreConditions();
        HashSet<string> GetPostEffects();
        void ResetConditionCache();
        bool IsProceduralConditionValid(Agent _agent);
        float GetCost();
    }

    [CreateAssetMenu(menuName = "Shared/Ai/GoapAction")]
    public class GoapActionConfig : ScriptableObject, I_GoapAction, ISerializationCallbackReceiver
    {
        public List<string> preConditions = new List<string>();
        public List<string> postEffects = new List<string>();

        public float baseCost = 1; 

        [NonSerialized]
        private bool conditionsValid = true;

        [NonSerialized]
        private bool conditionsChecked = false;

        [NonSerialized]
        private HashSet<string> preConditionsSet = new HashSet<string>();

        [NonSerialized]
        private HashSet<string> postEffectsSet = new HashSet<string>();

        public HashSet<string> GetPreConditions()
        {
            return preConditionsSet;
        }
        public HashSet<string> GetPostEffects()
        {
            return postEffectsSet;
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            //create the hashsets
            preConditionsSet.Clear();
            for (int i = 0; i < preConditions.Count; i++)
            {
                preConditionsSet.Add(preConditions[i]);
            }
            postEffectsSet.Clear();
            for (int i = 0; i < postEffects.Count; i++)
            {
                postEffectsSet.Add(postEffects[i]);
            }

        }

        public virtual void ResetConditionCache()
        {
            conditionsChecked = false;
        }

        public virtual bool CheckProceduralCondition(Agent _agent)
        {
            return true;
        }

        public bool IsProceduralConditionValid(Agent _agent)
        {
            if (!conditionsChecked)
            {
                conditionsValid = CheckProceduralCondition(_agent);
                conditionsChecked = true;
            }

            return conditionsValid;
        }

        public float GetCost()
        {
            return baseCost;
        }

        public object Clone()
        {
            return Instantiate(this);
        }
    }
}
