using System;
using System.Collections;
using System.Collections.Generic;

namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        public T Value { get; private set; }
        public BinaryTree<T> Left { get; private set; }
        public BinaryTree<T> Right { get; private set; }
        private readonly SortedSet<T> tree;

        public BinaryTree()
        {
            tree = new SortedSet<T>();
        }

        public void Add(T value)
        {
            if (tree.Count == 0)
                Value = value;
            else if (Value.CompareTo(value) >= 0)
            {
                Left = Left ?? new BinaryTree<T>();
                Left.Add(value);
            }
            else
            {
                Right = Right ?? new BinaryTree<T>();
                Right.Add(value);
            }
            tree.Add(value);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator() => tree.GetEnumerator();
    }

    public class BinaryTree
    {
        public static BinaryTree<T> Create<T>(params T[] values) where T : IComparable
        {
            var bt = new BinaryTree<T>();
            for (var i = 0; i < values.Length; i++)
                bt.Add(values[i]);
            return bt;
        }
    }
}