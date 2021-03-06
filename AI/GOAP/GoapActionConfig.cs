﻿using System;
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
        void InitPlanning(GoapPlanner.PlanningData _planningData);

        bool IsProceduralConditionValid();
        
        //This resets the conditions cache for the procedural conditions
        void ResetConditionCache();

        float GetCost();
        

        I_GoapAgent GetOwner();
        bool RequiresInRange();
        Vector3 GetTargetPosition();

        void Dispose();

        void ActionHasBeenPicked();
    }

    [CreateAssetMenu(menuName = "Shared/Ai/GoapAction")]
    public class GoapActionConfig : ScriptableObject, I_GoapAction, ISerializationCallbackReceiver
    {
        [Header("Goap Settings")]
       // public List<string> invalidPreConditions = new List<string>(); //these conditions make action not usable!
        public List<GoapEffect> preConditions = new List<GoapEffect>(); //these needs to be furfilled
        public List<GoapEffect> postEffects = new List<GoapEffect>(); //this is the result
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

        bool isDisposed = false;
        ~GoapActionConfig()
        {
            if (!isDisposed)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            sharedGoapData.counter--;
            isDisposed = true;
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
                //preConditionsSet.Add(preConditions[i]);
                preConditionsSet.Add(GoapEffect.IdNames[preConditions[i].value] );
            }
            postEffectsSet.Clear();
            for (int i = 0; i < postEffects.Count; i++)
            {
                postEffectsSet.Add(GoapEffect.IdNames[postEffects[i].value] );
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

            Debug.Log("cost: " + sharedGoapData.counter);
            return baseCost + sharedGoapData.counter;
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

        public virtual void InitPlanning(GoapPlanner.PlanningData _planningData)
        {
            owner = _planningData.goapAgent;
            ownerPos = _planningData.agentPositonXZ;
        }

        public virtual void ActionHasBeenPicked()
        {
            
        }
    }
}
