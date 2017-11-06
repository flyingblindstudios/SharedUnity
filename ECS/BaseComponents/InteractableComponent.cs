using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractableComponent : IComponent 
{
    public enum INTERACTION_TYPE { ON_CLICK = 0, ON_LONG_PRESS = 1, ON_CLICK_UP = 2};
    public enum LAYER_TYPE { PLANET = 0, BUILDING = 1};
    //List<InteractionComposition> m_Interactions = new List<InteractionComposition>();
	
	public bool m_IsInteractive = true;
	public LAYER_TYPE m_Layer; //make enum out of it?


    [System.Serializable]
    public class Interaction
    {
        public UnityEvent myUnityEvent;
        public INTERACTION_TYPE m_InteractionType;
    }

    public List<Interaction> m_Interactions;

}
