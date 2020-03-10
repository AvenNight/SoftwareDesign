using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.TreeTraversal
{
    public class Traversal
    {
        public static IEnumerable<T> Walk<T, TTree>(TTree tree,
            Func<TTree, IEnumerable<T>> traversal,
            Func<IEnumerable<T>, IEnumerable<T>> filter)
        {
            if (tree == null) return Enumerable.Empty<T>();
            var result = traversal(tree);
            return filter?.Invoke(result) ?? result;
        }

        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> tree)
        {
            return Walk(tree, t =>
                {
                    List<T> r = GetBinaryTreeValues(tree.Left).ToList();
                    r.Add(tree.Value);
                    r.AddRange(GetBinaryTreeValues(tree.Right));
                    return r;
                }, null);
        }

        public static IEnumerable<Job> GetEndJobs(Job tree)
        {
            return Walk(tree, t =>
                {
                    var r = new List<Job>() { tree };
                    foreach (var e in tree.Subjobs)
                        r.AddRange(GetEndJobs(e));
                    return r;
                },
                t => t.Where(x => x.Subjobs.Count == 0));
        }

        public static IEnumerable<Product> GetProducts(ProductCategory tree)
        {
            return Walk(tree, t =>
                {
                    var r = tree.Products;
                    foreach (var e in tree.Categories)
                        r.AddRange(e.Products);
                    return r;
                }, null);
        }
    }
}