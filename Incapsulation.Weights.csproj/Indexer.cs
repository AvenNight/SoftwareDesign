using System;

namespace Incapsulation.Weights
{
	internal class Indexer
	{
		private readonly double[] range;
		private readonly int start;
		public readonly int Length;
		public Indexer(double[] range, int start, int length)
		{
			this.range = range;
			if (start < 0 || length < 0 || (length + start) > range.Length)
				throw new ArgumentException();
			this.start = start;
			Length = length;
		}

		public double this[int i]
		{
			get
			{
				Check(i);
				return range[start + i];
			}
			set
			{
				Check(i);
				range[start + i] = value;
			}
		}

		private void Check(int i)
		{
			if (i < 0 || i > Length - 1)
				throw new IndexOutOfRangeException();
		}
	}
}