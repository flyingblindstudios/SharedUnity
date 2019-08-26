using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.AI
{ 
    public class GoapPlanner
    {
        //ToDo multithread this
        List<I_GoapAction> Plan(HashSet<string> _worldState, HashSet<string> _goalState, I_GoapActionSet _actions)
        {
            HashSet<string> goalSet = new HashSet<string>();

            goalSet.UnionWith(_goalState);

            //this gives me the set that wants to be satisfied
			goalSet.ExceptWith(_worldState);

            List<I_GoapAction> finalSequence = null;

            float smallestCost = float.MaxValue;

            //find all actions with statisfy the goal
            I_GoapAction[] allActions = _actions.GetActions();
            for (int i = 0; i < allActions.Length; i++)
            {
        
                HashSet<string> posteffects = allActions[i].GetPostEffects();

                if (goalSet.IsSubsetOf(posteffects))
                {
                    //action satisfies the goal
                    List<I_GoapAction> actionSequence = new List<I_GoapAction>();
                    float cost = 0;

                    actionSequence.Add(allActions[i]);

                    if (FindActionSequence(ref actionSequence, ref cost, allActions, allActions[i].GetPreConditions()))
                    {
                        if(cost < smallestCost)
                        {
                            smallestCost = cost;
                            //found sutible action sequence
                            finalSequence = actionSequence;
                        }
                    }

                }
            }

			return finalSequence;
        }

        //returns if it can satisfy action chain
        bool FindActionSequence(ref List<I_GoapAction> _actionSequence, ref float _cost, I_GoapAction[] _availableActions, HashSet<string> _goal)
        {
            for (int i = 0; i < _availableActions.Length; i++)
            {
                //if action os already in squence contnue? this wont work with multiple go to actions..
                if (_actionSequence.Contains(_availableActions[i]))
                {
                    continue;
                }

                HashSet<string> posteffects = _availableActions[i].GetPostEffects();

                if (goalSet.IsSubsetOf(posteffects))
                {
                }
            }
        }

    }

}