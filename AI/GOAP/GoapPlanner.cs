using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Data;

namespace Shared.AI
{
    public class GoapPlanner
    {

        //Creates a tree from an actionset and a goal
        public TreeNode<I_GoapAction> CreateTree(HashSet<string> _goalState, I_GoapActionSet _actionSet )
        {
            return CreateTree(_goalState, _actionSet.GetActions());
        }

        public TreeNode<I_GoapAction> CreateTree(HashSet<string> _goalState, I_GoapAction[] _actions)
        {
            //the root node has no action
            TreeNode<I_GoapAction> rootNode = new TreeNode<I_GoapAction>(null);

            List<I_GoapAction> relevantActions = new List<I_GoapAction>();

            //find all actions relavant to the goal?
            for (int i = 0; i < _actions.Length; i++)
            {
                if (_actions[i].GetPostEffects().Count > 0 && _actions[i].GetPostEffects().IsSubsetOf(_goalState))
                {
                    relevantActions.Add(_actions[i]);
                }
            }



            List<TreeNode<I_GoapAction>> nodesAdded = new List<TreeNode<I_GoapAction>>();
            //find all permutations that satisfy the goal? //combine all actions inside the tree
            for (int i = 0; i < relevantActions.Count; i++)
            {
                TreeNode<I_GoapAction> activeNode = rootNode;
                //this is what we still need to to do
                HashSet<string> tmpGoalSet = new HashSet<string>();
                tmpGoalSet.UnionWith(_goalState);
                tmpGoalSet.ExceptWith(relevantActions[i].GetPostEffects());

                TreeNode<I_GoapAction> treeThisAction = activeNode.Add(relevantActions[i]);
                nodesAdded.Add(treeThisAction);
                for (int x = 0; x < relevantActions.Count; x++)
                {
                    //if our action then continue
                    if (relevantActions[x] == relevantActions[i])
                    {
                        continue;
                    }

                    if (relevantActions[x].GetPostEffects().IsSubsetOf(tmpGoalSet))
                    {
                        //we want this action
                        tmpGoalSet.ExceptWith(relevantActions[x].GetPostEffects());
                        activeNode = activeNode.Add(relevantActions[x]);
                        nodesAdded.Add(activeNode);
                    }
                }
            }

            //fill up all requirements
            for (int i = 0; i < nodesAdded.Count; i++)
            {
                TreeNode<I_GoapAction> treeThisAction = CreateTree(nodesAdded[i].GetValue().GetPreConditions(), _actions);
                nodesAdded[i].AddTree(treeThisAction);
            }

            return rootNode;
        }


        //ToDo multithread this
        /*public List<I_GoapAction> Plan(HashSet<string> _worldState, HashSet<string> _goalState, TreeNode<I_GoapAction> _actionTree)
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
        }*/

    }

}