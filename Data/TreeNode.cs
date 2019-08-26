﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shared.Data
{
    public class TreeNode<T>
    {
        public TreeNode<T> parent = null;
        T m_Value;

        List<TreeNode<T>> childs = new List<TreeNode<T>>();

        public TreeNode(T _value)
        {
            m_Value = _value;
        }

        public TreeNode<T> Add(T _value)
        {
            TreeNode<T> node = new TreeNode<T>(_value);
            childs.Add(node);
            node.parent = this;
            return node;
        }

        public int ChildCount()
        {
            return childs.Count;
        }

        public T GetValue()
        {
            return m_Value;
        }

        public void AddTree(TreeNode<T>  _tree)
        {
            for (int i = 0; i < _tree.ChildCount(); i++)
            {

            }
        }
    }
}