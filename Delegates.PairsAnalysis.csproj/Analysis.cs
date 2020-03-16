using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            var pairs = data.Pairs();
            var periods = pairs.Select(p => Math.Abs((p.Item2 - p.Item1).TotalSeconds));
            return periods.MaxIndex();
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            var pairs = data.Pairs();
            var deltas = pairs.Select(p => (p.Item2 - p.Item1) / p.Item1);
            return deltas.Sum() / deltas.Count();
        }

        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> sequence) where T : struct
        {
            T? temp = null;
            foreach (var e in sequence)
            {
                if (temp != null)
                    yield return Tuple.Create(temp ?? default, e);
                temp = e;
            }
        }

        public static int MaxIndex<T>(this IEnumerable<T> sequence) where T : IComparable
        {
            int i = 0, maxIndex = -1;
            T maxValue = default;
            foreach (var e in sequence)
            {
                if (i == 0) maxValue = e;
                if (e.CompareTo(maxValue) > 0)
                {
                    maxValue = e;
                    maxIndex = i;
                }
                i++;
            }
            if (i == 0) throw new ArgumentException();
            return maxIndex;
        }
    }
}