using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Data;

namespace Shared.AI
{
    public class GoapPlanner
    {

        //returns true if finds leave nodes
        public static bool BuildTree(GoapNode _parentNode, List<GoapNode> _leaves, List<I_GoapAction> _remainingActions, HashSet<string> _goalState)
        {
            bool foundSolution = false;

            for (int i = 0; i < _remainingActions.Count; i++)
            {
                //check if we can use this action
                if (_remainingActions[i].GetPreConditions().IsSubsetOf(_parentNode.currentState))
                {
					Debug.Log("Found valid action!");
                    //found an action, create node and build with build tree with remaining actions
                    GoapNode newNode = new GoapNode(_remainingActions[i]);
                    newNode.parent = _parentNode;
                    newNode.currentState.UnionWith(_parentNode.currentState);
                    newNode.currentState.UnionWith(_remainingActions[i].GetPostEffects());
                    newNode.runningCost = _parentNode.runningCost + _remainingActions[i].GetCost();

                    


                    if (_goalState.IsSubsetOf(newNode.currentState))
                    {
                        //found a solution
                        foundSolution = true;
                        _leaves.Add(newNode);
                        Debug.Log("Found a solution!");
                    }
                    else
                    {
                        List<I_GoapAction> remainingActions = new List<I_GoapAction>(_remainingActions);
                        remainingActions.RemoveAt(i); //remove used action

                        foundSolution = BuildTree(newNode, _leaves, remainingActions, _goalState);
                    }
                }
            }
            

            return foundSolution;
        }

        public static List<I_GoapAction> PlanDynamic(Agent _agent, HashSet<string> _worldState, HashSet<string> _goalState, I_GoapActionSet _actionSet)
        {
            //Check is there an action which can just run on the current worldstate without building a tree?


            //sort by number of effects
            List<I_GoapAction> actions = new List<I_GoapAction>(_actionSet.GetActions());

            ///////TODO!!! Sort by number of preconditions


            //this is returned
            List<I_GoapAction> acitonSequence = new List<I_GoapAction>();

            //check if one action can just satisfy
            /*for (int i = 0; i < actions.Count; i++)
            {
                actions[i].ResetConditionCache();
                if (actions[i].GetPreConditions().IsSubsetOf(_worldState) && actions[i].IsProceduralConditionValid(_agent))
                {
                    HashSet<string> newWorldState = new HashSet<string>();
                    newWorldState.UnionWith(_worldState);
                    newWorldState.UnionWith(actions[i].GetPostEffects());

                    if (_goalState.IsSubsetOf(newWorldState))
                    {
                        acitonSequence.Add(actions[i]);
                        return acitonSequence;
                    }

                }
            }*/


            List<GoapNode> leaves = new List<GoapNode>(); //leaves are the solutions

            GoapNode rootNode = new GoapNode(null, null,_worldState,0);

            
            bool success = BuildTree(rootNode, leaves, actions, _goalState);


            if (success)
            {
                Debug.Log("Found solution");
            }


            //return goap actions with cheapest cost
            float smallestCost = float.MaxValue;
            int index = -1;
            for (int i = 0; i < leaves.Count; i++)
            {
                if (smallestCost > leaves[i].runningCost)
                {
                    index = i;
                }
            }

			Debug.Log("Found Solutions: " + leaves.Count);

            if(leaves.Count > 0)
			{ 
                GoapNode currentNode = leaves[index];
                while (currentNode != null)
                {
                    if(currentNode.GetValue() != null)
                    { 
                        acitonSequence.Insert(0,(I_GoapAction)currentNode.GetValue().Clone());
                    }
                    currentNode = (GoapNode)currentNode.parent;
                }
			}
			return acitonSequence;
        }
    }

}