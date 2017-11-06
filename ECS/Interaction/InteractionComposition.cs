using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(InteractableComponent))]
public class InteractionComposition : MonoBehaviour {

	
	
	// Use this for initialization
	void Awake () 
	{
		//GetComponent<InteractableComponent>().AddInteraction(this);
	}
	
	// Update is called once per frame
	void OnDestroy () 
	{
		//GetComponent<InteractableComponent>().RemoveInteraction(this);
	}
}
