using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyComponent : IComponent {

	public Rigidbody m_Rigidbody = null;
	
	protected override void OnAwake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
	}
}
