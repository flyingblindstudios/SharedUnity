using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;

namespace Shared.AI
{
    public class GoapPlanner : Singleton<GoapPlanner>
    {
        ConcurrentQueue<GoapRequest> pendingRequests = new ConcurrentQueue<GoapRequest>();

        protected class GoapRequest
        {
            public Vector2 agentPos;
            public I_GoapAction[] actions;
            public HashSet<string> worldState;
            public HashSet<string> goalState;
            public I_GoapAgent owner;
            public Action<List<I_GoapAction>> callback;
            public GoapRequest(I_GoapAgent _agent, I_GoapAction[] _actions, HashSet<string> _worldState, HashSet<string> _goalState, Action<List<I_GoapAction>> _callback)
            {
                owner = _agent;
                agentPos = _agent.GetPositionXZ();
                actions = _actions;
                worldState = _worldState;
                goalState = _goalState;
                callback = _callback;
            }    
        }

        public void PlanAsync(I_GoapAgent _agent, HashSet<string> _worldState, HashSet<string> _goalState, I_GoapActionSet _actionSet, Action<List<I_GoapAction>> _callback)
        {
            Vector2 agentPos = _agent.GetPositionXZ();
            I_GoapAction[] actions = _actionSet.GetActions();
            GoapRequest gRequest = new GoapRequest(_agent, actions, _worldState, _goalState, _callback);

            //thread safe enqueue
            pendingRequests.Enqueue(gRequest);
        }


        private void Update()
        {
            if (!pendingRequests.IsEmpty)
            {
                GoapRequest gRequest;
                if (pendingRequests.TryDequeue(out gRequest))
                {
                    ExecuteGoapRequest(gRequest);
                }     
            }
        }


        //returns true if finds leave nodes
        private static bool BuildTree(GoapRequest _request,GoapNode _parentNode, List<GoapNode> _leaves, List<I_GoapAction> _remainingActions, HashSet<string> _goalState)
        {
            bool foundSolution = false;

            for (int i = 0; i < _remainingActions.Count; i++)
            {

                _remainingActions[i].InitPlanning(_request.owner, _request.agentPos);

                //check if we can use this action
                if (_remainingActions[i].GetPreConditions().IsSubsetOf(_parentNode.currentState) && _remainingActions[i].IsProceduralConditionValid())
                {
					//Debug.Log("Found valid action!");
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
                        //Debug.Log("Found a solution!");
                    }
                    else
                    {
                        List<I_GoapAction> remainingActions = new List<I_GoapAction>(_remainingActions);
                        remainingActions.RemoveAt(i); //remove used action

                        foundSolution = BuildTree(_request, newNode, _leaves, remainingActions, _goalState);
                    }
                }
            }
            

            return foundSolution;
        }

        private static void ExecuteGoapRequest(GoapRequest _request)
        {
            //I_GoapAgent _agent, HashSet< string > _worldState, HashSet<string> _goalState, I_GoapActionSet _actionSet

            //Check is there an action which can just run on the current worldstate without building a tree?


            //sort by number of effects
            List<I_GoapAction> actions = new List<I_GoapAction>(_request.actions);

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

            GoapNode rootNode = new GoapNode(null, null, _request.worldState, 0);

            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i] == null)
                {
                    Debug.LogError("One of the ations is null!");
                }
                actions[i].ResetConditionCache();
            }

            
            bool success = BuildTree(_request, rootNode, leaves, actions, _request.goalState);


            if (success)
            {
                Debug.Log("Found solution");
            }


            //return goap actions with cheapest cost
            float smallestCost = float.MaxValue;
            int index = -1;
           // Debug.Log("found leaves: " + leaves.Count);
            for (int i = 0; i < leaves.Count; i++)
            {
                
                //Debug.Log("cost: " + leaves[i].runningCost);
                if (smallestCost > leaves[i].runningCost)
                {
                    index = i;
                    smallestCost = leaves[i].runningCost;
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
                        I_GoapAction action = (I_GoapAction)currentNode.GetValue().Clone();
                        action.ActionHasBeenPicked();
                        acitonSequence.Insert(0, action);
                    }
                    currentNode = (GoapNode)currentNode.parent;
                }
			}

            //callback -> should be executed on the main thread if this is ever threaded!
            _request.callback?.Invoke(acitonSequence);
        }
    }

}