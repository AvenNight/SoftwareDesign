using System.Drawing;

namespace MyPhotoshop
{
	public class Photo
	{
		public readonly int Width;
		public int Height { get; } // тоже самое ридонли ток поле, по идее щас так лучше
		private readonly Pixel[,] data;

		public Photo(int width, int height)
		{
			Width = width;
			Height = height;
			data = new Pixel[width, height];
		}

		public Photo(Size size) : this (size.Width, size.Height){ }

		public Photo(Bitmap bmp) : this(bmp.Width, bmp.Height)
		{
			for (int x = 0; x < bmp.Width; x++)
				for (int y = 0; y < bmp.Height; y++)
				{
					var pixel = bmp.GetPixel(x, y);
					this[x, y] = new Pixel(
						(double)pixel.R / 255,
						(double)pixel.G / 255,
						(double)pixel.B / 255);
				}
		}

		public Pixel this[int x, int y]
		{
			set => data[x, y] = value;
			get => data[x, y];
		}
	}
}