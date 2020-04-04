using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyPhotoshop
{
	class MainClass
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var window = new MainWindow();
			window.AddFilter(new PixelFilter<LighteningParameters>(
					"Осветление/затемнение",
					(original, parameters) => original * parameters.Coefficient
				));
			window.AddFilter(new PixelFilter<GrayscaleParameters>(
				"Оттенки серого",
				(original, parameters) =>
				{
					var average = 0.3 * original.R + 0.587 * original.G + 0.114 * original.B;

					return new Pixel(
						Pixel.Trim(original.R * (1 - parameters.Coefficient) + average * parameters.Coefficient),
						Pixel.Trim(original.G * (1 - parameters.Coefficient) + average * parameters.Coefficient),
						Pixel.Trim(original.B * (1 - parameters.Coefficient) + average * parameters.Coefficient));
				}
				));
			window.AddFilter(new TransformFilter(
				"Отражение по горизонтали",
				oldSize => oldSize,
				(oldPoint, oldSize) => new Point(oldSize.Width - 1 - oldPoint.X, oldPoint.Y)
				));
			window.AddFilter(new TransformFilter(
				"Отражение по вертикали",
				oldSize => oldSize,
				(oldPoint, oldSize) => new Point(oldPoint.X, oldSize.Height - 1 - oldPoint.Y)
				));
			window.AddFilter(new TransformFilter(
				"Поворот по ч.с.",
				oldSize => new Size(oldSize.Height, oldSize.Width),
				(oldPoint, oldSize) => new Point(oldPoint.Y, oldSize.Height - 1 - oldPoint.X)
				));
			window.AddFilter(new TransformFilter<RotationParameters>("Свободное вращение", new RotateTransformer()));
			Application.Run(window);
		}
	}
}