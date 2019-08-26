using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shared.Data
{
    public class TreeNode<T>
    {
        T m_Value;

        List<TreeNode<T>> childs = new List<TreeNode<T>>();

        public TreeNode(T _value)
        {
            m_Value = _value;
        }

        public void Add(T _value)
        {
            childs.Add(new TreeNode<T>(_value));
        }

        int ChildCount()
        {
            return childs.Count;
        }
    }
}