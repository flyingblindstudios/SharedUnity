using Shared.Data;
using System.Collections.Generic;
namespace Shared.AI
{ 
    public class GoapNode : TreeNode<I_GoapAction>
    {
        public HashSet<string> currentState = new HashSet<string>();
        public float runningCost = 0;

        public GoapNode(I_GoapAction _value): base(_value)
        {

        }

        public GoapNode(I_GoapAction _value, GoapNode _parent, HashSet<string> _baseState, float _runningCost) : base(_value, _parent)
        {
            runningCost = _runningCost;
            currentState = _baseState;
        }
    }
}