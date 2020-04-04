using System;

namespace MyPhotoshop
{
	public class PixelFilter<TParams> : ParametrizedFilter<TParams> where TParams : IParameters, new()
	{
		protected Func<Pixel, TParams, Pixel> processPixel;

		public PixelFilter(string name, Func<Pixel, TParams, Pixel> processPixel)
		{
			this.name = name;
			this.processPixel = processPixel;
		}

		public override Photo Process(Photo original, TParams parameters)
		{
			var result = new Photo(original.Width, original.Height);

			for (int x = 0; x < result.Width; x++)
				for (int y = 0; y < result.Height; y++)
					result[x, y] = processPixel(original[x, y], parameters);

			return result;
		}
	}
}