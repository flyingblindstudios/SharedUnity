using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNormalGizmos : MonoBehaviour {

	
	private Vector3[] m_Normals = null;
	private Vector3[] m_Vertices;

	private Mesh m_Mesh;
	// Use this for initialization
	void Start () {
		
		m_Mesh = GetComponent<MeshFilter>().mesh;
		m_Mesh.name = "Procedural Cube";
		m_Vertices = m_Mesh.vertices;
		m_Normals = m_Mesh.normals;


		for(int i = 0; i < m_Mesh.tangents.Length;i++)
		{
			Debug.Log("base: "+ m_Mesh.tangents[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private void OnDrawGizmos () {


		if (m_Normals == null) {
			
			return;
		}
		for (int i = 0; i < m_Normals.Length; i++) {

//			Debug.Log("m_Normals");
			if(m_Normals[i] == null)
			{
				break;

			}

			Gizmos.DrawLine(m_Vertices[i], m_Vertices[i]+m_Normals[i] );
		}


		
	}
}
