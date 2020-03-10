//using System.Collections.Generic;
//using System.Linq;

//namespace Delegates.TreeTraversal
//{
//    public class Traversal
//    {
//        public static List<T> GetBinaryTreeValues<T>(BinaryTree<T> tree)
//        {
//            if (tree == null) return new List<T>();
//            var result = GetBinaryTreeValues(tree.Left);
//            result.Add(tree.Value);
//            result.AddRange(GetBinaryTreeValues(tree.Right));
//            return result;
//        }

//        public static List<Job> GetEndJobs(Job tree)
//        {
//            if (tree == null) return new List<Job>();
//            var result = new List<Job>() { tree };
//            foreach (var e in tree.Subjobs)
//                result.AddRange(GetEndJobs(e));
//            result = result.Where(x => x.Subjobs.Count == 0).ToList();
//            return result;
//        }

//        public static List<Product> GetProducts(ProductCategory tree)
//        {
//            if (tree == null) return new List<Product>();
//            var result = tree.Products;
//            foreach (var e in tree.Categories)
//                result.AddRange(e.Products);
//            return result;
//        }
//    }
//}