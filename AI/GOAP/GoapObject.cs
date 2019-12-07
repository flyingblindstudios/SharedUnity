using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Shared.AI
{
    [Serializable]
    public class GoapAgentEvent : UnityEvent<I_GoapAgent>
    {

    }

    public class GoapObject : MonoBehaviour
    {
        [SerializeField] private GoapID objectLayer;
        [SerializeField] private string objectTag = "";

        [SerializeField] private List<GoapEffect> worldEffects = new List<GoapEffect>();

        [SerializeField] private int maxAgents = 1;

        [SerializeField] private bool canBeUsed = true;

        [Header("UnityEvents")]
        [SerializeField] protected UnityEvent onEnter;
        [SerializeField] protected UnityEvent onExit;
        [SerializeField] protected GoapAgentEvent onEnterWithAgent;
        [SerializeField] protected GoapAgentEvent onExitWithAgent;

        private int inUse = 0;
        private Vector3 positionCache;
        private Vector2 positionCacheXZ;

        
        private int tagHash;

        private void Start()
        {
            positionCache = this.transform.position;
            positionCacheXZ = Shared.MathUtil.GetVectorXZ(positionCache);

            tagHash = objectTag.GetHashCode();
        }

        private void OnEnable()
        {
            GoapWorldManager.GetInstance().AddObject(this);
            positionCache = this.transform.position;
            positionCacheXZ = Shared.MathUtil.GetVectorXZ(positionCache);
        }

        private void OnDisable()
        {
            if (GoapWorldManager.GetInstance())
            {
                GoapWorldManager.GetInstance().RemoveObject(this);
            }
        }

        public Vector3 GetPosition()
        {
            return positionCache;
        }

        public Vector2 GetPositionXZ()
        {
            return positionCacheXZ;
        }

        /*public void AddGoapEffect(GoapEffectID _effectID)
        {
            //needs to update the worldstate mananger

            //forward to GoapWorldmanager -> something added, something removed
        }*/

        public bool CanBeUsed()
        {
            if (canBeUsed && (inUse < maxAgents || maxAgents < 0))
            {
                return true;
            }
            return false;
        }

        public void InUseUp()
        {
            inUse++;
        }
    
        public void InUseDown()
        {
            inUse--;
        }


        public void OnActionEnter(GoapSmartActionConfig _smartAction)
        {
            onEnter?.Invoke();
            onEnterWithAgent?.Invoke(_smartAction.GetOwner());
        }

        public void OnActionExit(GoapSmartActionConfig _smartAction)
        {
            onExit?.Invoke();
            onExitWithAgent?.Invoke(_smartAction.GetOwner());
        }

        public GoapID GetObjectLayer()
        {
            return objectLayer;
        }

        public List<GoapEffect> GetWorldEffects()
        {
            return worldEffects;
        }

        public int GetTagHash()
        {
            return tagHash;
        }
        
    }
}