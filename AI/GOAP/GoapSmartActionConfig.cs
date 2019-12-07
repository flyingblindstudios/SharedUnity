using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Shared.AI
{
    [CreateAssetMenu(menuName = "Shared/Ai/SmartAction")]
    public class GoapSmartActionConfig : StateGoapActionConfig
    {
        [SerializeField] protected GoapID queryObjectsID;
        [SerializeField] protected string queryObjectTag;

        
        protected GoapObject objectTarget;
        protected float sqrDistanceToTarget;

        protected float enterTime;
        protected int objectTag;


        public override void InitPlanning(GoapPlanner.PlanningData _planningData)
        {
            base.InitPlanning(_planningData);
            objectTarget = GoapWorldManager.GetInstance().QueryClosestObjectWithID(queryObjectsID, objectTag, _planningData.agentPositonXZ, out sqrDistanceToTarget);
            objectTag = queryObjectTag.GetHashCode();
        }

        public override Vector3 GetTargetPosition()
        {
            return objectTarget.GetPosition();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            Debug.Log("OnStateEnter");

            /*GoapAgent goapAgent = (GoapAgent)GetOwner();
            goapAgent.GetAnimationComponent().PlayGroundInteraction();*/


            //play animation
            //goapAgent.GetAnimationComponent().PlayGroundInteraction();
            enterTime = Time.time;

            objectTarget.OnActionEnter(this);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (!CheckProceduralCondition())
            {
                Break();
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            objectTarget.InUseDown();

            objectTarget.OnActionExit(this);
        }

        public override void OnStateAbort()
        {
            base.OnStateAbort();
            objectTarget.InUseDown();
        }

        public override float GetCost()
        {
            return base.GetCost();
        }

        public override bool CheckProceduralCondition()
        {
            if (objectTarget == null)
            {
                return false;
            }
            //check if agent has something in storage
            return true;
        }

        public override bool IsDone()
        {
            return (Time.time - enterTime) > 2.0f;
        }

        public override void ActionHasBeenPicked()
        {
            base.ActionHasBeenPicked();
            if (objectTarget)
            {
                objectTarget.InUseUp();
            }
        }

    }
}
