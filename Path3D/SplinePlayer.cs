using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinePlayer : MonoBehaviour {

   

    [SerializeField]
    Path3D m_Path;

    [SerializeField]
    Rigidbody m_Ridig;

    Path3D.SplinePoint myPoint;
    Path3D.SplinePoint targetPoint;

    Vector3 currentForward;

    // Use this for initialization
    void Start () {
        SnapToPath();
	}
	
	// Update is called once per frame
	void Update () {
        Move();

    }


    void SnapToPath()
    {
        Path3D.SplinePoint sp = m_Path.GetClosestPoint(this.transform.position);

        this.transform.position = sp.pos;
        this.transform.rotation = Quaternion.LookRotation(sp.forward, Vector3.up);
    }


    void Move()
    {
        myPoint = m_Path.GetClosestPoint(this.transform.position);
        

        if (Input.GetKey(KeyCode.A))
        {
            targetPoint = m_Path.GetClosestPoint(myPoint.pos + myPoint.forward); // this depends on direction

            currentForward = (targetPoint.pos - this.transform.position).normalized;

            m_Ridig.AddForce((currentForward * 20.0f*Time.deltaTime)- m_Ridig.velocity, ForceMode.VelocityChange);
        } else if (Input.GetKey(KeyCode.D))
        {
            targetPoint = m_Path.GetClosestPoint(myPoint.pos - myPoint.forward); // this depends on direction
            currentForward = (targetPoint.pos - this.transform.position).normalized;
            m_Ridig.AddForce((currentForward * 20.0f * Time.deltaTime) - m_Ridig.velocity, ForceMode.VelocityChange);
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Gizmos.DrawLine(this.transform.position, myPoint.pos);

    }


}
