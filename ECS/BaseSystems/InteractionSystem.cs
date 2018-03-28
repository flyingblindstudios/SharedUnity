using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
namespace ECS.Systems
{
	//should process by layer -> layer doesnt make senses remove layer again
	//should maybe use worldpointtoscreenpoint?
	//if useing raycast
	//only raycast of there has been in interaction we care about

	public class InteractionSystem : ISystem  
	{

		
		readonly Type[] _acceptedNodes = { typeof(InteractableComponent)};
		EntityDescriptor m_RaycastObject = null;
	//	Dictionary<InteractableComponent.INTERACTION_TYPE, bool> m_InteractiontypesChecker = new Dictionary<InteractableComponent.INTERACTION_TYPE, bool>();
		

		
		// Update is called once per frame
		protected override void OnUpdate () {
			
			m_RaycastObject = null;
			if(!CheckInteractionType(InteractableComponent.INTERACTION_TYPE.ON_CLICK) && !CheckInteractionType(InteractableComponent.INTERACTION_TYPE.ON_LONG_PRESS) && !CheckInteractionType(InteractableComponent.INTERACTION_TYPE.ON_CLICK_UP))
			{
				return;				
			}
			
			
		

			
			EntityDescriptor desc = FindRayCastObject();
			if(desc != null)
			{	
				InteractableComponent interactable = null;
				Type interactableType = typeof(InteractableComponent);
	
				interactable = (InteractableComponent)desc.m_IComponents[interactableType];
				if(interactable.m_IsInteractive)
				{
					Debug.Log("Interaction with: " + desc.name);
					ProcessInteractions(interactable);
				}
			}

			
		}

		void ProcessInteractions(InteractableComponent _interactable)
		{
			//if( interactable.inter InteractableComponent.INTERACTION_TYPE
			for(int i = 0; i < _interactable.m_Interactions.Count;i++)
			{
				if(CheckInteractionType(_interactable.m_Interactions[i].m_InteractionType) )
				{
					UnityEvent uEvent = _interactable.m_Interactions[i].myUnityEvent;
					uEvent.Invoke();
				}
			}		
		}

		bool CheckInteractionType(InteractableComponent.INTERACTION_TYPE _interactionType)
		{
			if(_interactionType == InteractableComponent.INTERACTION_TYPE.ON_CLICK)
			{
				TouchTracker tm = TouchInputManager.GetInstance().m_TouchTracker[0];
				if(tm.GetTouchBegan())
				{
					return true;
				}
			}
			else if(_interactionType == InteractableComponent.INTERACTION_TYPE.ON_LONG_PRESS)
			{
				
				TouchTracker tm = TouchInputManager.GetInstance().m_TouchTracker[0];
				if(tm.m_TouchDidBegin && !tm.m_TouchDidMove && tm.GetTimeSinceTouchBegan() > 0.5f)
				{
					return true;
				}
			}
			else if(_interactionType == InteractableComponent.INTERACTION_TYPE.ON_CLICK_UP)
			{
				TouchTracker tm = TouchInputManager.GetInstance().m_TouchTracker[0];
				if(tm.GetTouchEnded())
				{
					return true;
				}
			}
			return false;
		}

		EntityDescriptor FindRayCastObject()
		{
			if(m_RaycastObject)
			{
				return m_RaycastObject;
			}

			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			
			//stores the best hit / closest hit
			RaycastHit currentHit = new RaycastHit();
			currentHit.distance = float.MaxValue;

			for(int i = 0; i < m_Entities.Count;i++)
			{
				Collider collider = m_Entities[i].gameObject.GetComponent<Collider>();
				if(collider != null && collider.Raycast(ray, out hitInfo,1000.0f))
				{
					
					if(hitInfo.distance < currentHit.distance)
					{
						
						currentHit = hitInfo;
						m_RaycastObject = m_Entities[i];
						Debug.Log("found hit " + m_RaycastObject.name);
					}
				}
			}


			return m_RaycastObject;
		}

		public override Type[] AcceptedNodes()
		{
			return _acceptedNodes;

		}


	}
}