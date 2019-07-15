using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class UnityNavAgent : MonoBehaviour, I_NavAgent
{
    [SerializeField]
    float m_WalkingSpeed = 0.35f;

    [SerializeField]
    float m_RunningSpeed = 0.7f;

    NavMeshAgent m_UnityAgent;

    bool m_Running = false;

    private void Start()
    {
        m_UnityAgent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Vector3 _targetPoint, bool _running = false)
    {
        if (_running)
        {
            m_UnityAgent.speed = GetRunningSpeed();
        }
        else
        {
            m_UnityAgent.speed = GetWalkingSpeed();
        }
        m_Running = _running;
        m_UnityAgent.SetDestination(_targetPoint);
    }

    public bool IsRunning()
    {
        return m_Running;
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

    public float GetWalkingSpeed()
    {
        return m_WalkingSpeed;
    }

    public float GetRunningSpeed()
    {
        return m_RunningSpeed;
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
