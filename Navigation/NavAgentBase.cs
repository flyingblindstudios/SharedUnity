using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_NavAgent
{
    void SetTarget(Vector3 _targetPoint, bool _running = false);
    bool HasReachedDestination();
    Vector3 GetDirection();
    Vector3 GetNormal();
    Vector3 GetPosition();
    Vector3 GetTargetPosition();
    float GetWalkingSpeed();
    float GetRunningSpeed();
    bool IsRunning();
    float GetRemainingDistance();
}
