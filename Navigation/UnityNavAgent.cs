using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class UnityNavAgent : MonoBehaviour, I_NavAgent
{
    NavMeshAgent m_UnityAgent;

    private void Start()
    {
        m_UnityAgent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Vector3 _targetPoint)
    {
        m_UnityAgent.SetDestination(_targetPoint);
    }

    public bool HasReachedDestination()
    {
        float dist = m_UnityAgent.remainingDistance;

        /*This is a hack! The code under this didnt work somehow prop.*/
        if( Vector3.SqrMagnitude(m_UnityAgent.destination - this.transform.position) <= 0.001f)
        {
            return true;
        }

        /*if (dist != Mathf.Infinity && m_UnityAgent.pathStatus == NavMeshPathStatus.PathComplete && dist == 0)
        {
            return true;
        }*/

        return false;
    }

    public Vector3 GetDirection()
    {
        return m_UnityAgent.velocity.normalized;
    }

    public Vector3 GetNormal()
    {
        return Vector3.up;
    }

    public Vector3 GetPosition()
    {
        return m_UnityAgent.transform.position;
    }

    public Vector3 GetTargetPosition()
    {
        return m_UnityAgent.destination;
    }

    public void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Gizmos.DrawWireSphere( this.GetTargetPosition() ,0.1f);
        Gizmos.DrawLine(this.GetPosition(), this.GetTargetPosition());
    }

}
