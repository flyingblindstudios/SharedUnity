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

        //this is called by the planner to initilize relevent data
        void InitPlanning(I_GoapAgent _agent, Vector2 _position);

        bool IsProceduralConditionValid();
        
        //This resets the conditions cache for the procedural conditions
        void ResetConditionCache();

        float GetCost();
        

        I_GoapAgent GetOwner();
        bool RequiresInRange();
        Vector3 GetTargetPosition();

        
    }

    [CreateAssetMenu(menuName = "Shared/Ai/GoapAction")]
    public class GoapActionConfig : ScriptableObject, I_GoapAction, ISerializationCallbackReceiver
    {
        [Header("Goap Settings")]
        public List<string> preConditions = new List<string>();
        public List<string> postEffects = new List<string>();
        public bool requiresInRange = false;
        public float baseCost = 1;

        //how many agents are using the action right now in there goap execution? This number is not the ground through. It will be delayed.
        [NonSerialized] public SharedGoapActionData sharedGoapData = new SharedGoapActionData();

        [NonSerialized] private bool conditionsValid = true;

        [NonSerialized] private bool conditionsChecked = false;

        [NonSerialized] private HashSet<string> preConditionsSet = new HashSet<string>();

        [NonSerialized] private HashSet<string> postEffectsSet = new HashSet<string>();

        [NonSerialized] private I_GoapAgent owner;

        [NonSerialized] private Vector2 ownerPos;


        ~GoapActionConfig()
        {
            sharedGoapData.counter--;
        }

        public I_GoapAgent GetOwner()
        {
            return owner;
        }

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

        public virtual bool CheckProceduralCondition()
        {
            return true;
        }


        //final
        public bool IsProceduralConditionValid()
        {
            //this is here to ensure during planing for the same action the condition is only validated once
            if (!conditionsChecked)
            {
                conditionsValid = CheckProceduralCondition();
                conditionsChecked = true;
            }

            return conditionsValid;
        }

        public virtual float GetCost()
        {
            return baseCost;
        }

        public object Clone()
        {
            sharedGoapData.counter++;
            return this.MemberwiseClone();
            //return Instantiate(this);
        }

        public bool RequiresInRange()
        {
            return requiresInRange;
        }

        public virtual Vector3 GetTargetPosition()
        {
            return Vector3.zero;
        }

        public virtual void InitPlanning(I_GoapAgent _agent, Vector2 _agentPos)
        {
            owner = _agent;
            ownerPos = _agentPos;
        }
 
    }
}
