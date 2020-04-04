using System;
using System.Drawing;

namespace MyPhotoshop
{
	public class TransformFilter<TParams> : ParametrizedFilter<TParams> where TParams : IParameters, new()
	{
		private ITransformer<TParams> transformer;

		public TransformFilter(string name, ITransformer<TParams> transformer)
		{
			this.name = name;
			this.transformer = transformer;
		}

		public override Photo Process(Photo original, TParams parameters)
		{
			var oldSize = new Size(original.Width, original.Height);
			transformer.Prepere(oldSize, parameters);
			var result = new Photo(transformer.ResultSize);
			for (int x = 0; x < result.Width; x++)
				for (int y = 0; y < result.Height; y++)
				{
					var point = new Point(x, y);
					var oldPoint = transformer.MapPoint(point);
					if (oldPoint.HasValue)
						result[x, y] = original[oldPoint.Value.X, oldPoint.Value.Y];
				}
			return result;
		}
	}
}