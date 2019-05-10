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

        if (dist != Mathf.Infinity && m_UnityAgent.pathStatus == NavMeshPathStatus.PathComplete && dist == 0)
        {
            return true;
        }

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
}
