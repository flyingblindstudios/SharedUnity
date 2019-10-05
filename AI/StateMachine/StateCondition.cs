using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StateCondition
{
    bool IsSatisfied(); //if true statemachine will loop
}
