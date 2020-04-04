using System;
using System.Drawing;

namespace MyPhotoshop
{
	public class RotateTransformer : ITransformer<RotationParameters>
	{
		public Size OriginalSize { get; private set; }
		public Size ResultSize { get; private set; }
		public double Angle { get; private set; }

		public void Prepere(Size oldSize, RotationParameters @params)
		{
			Angle = Math.PI * @params.Angle / 180;
			OriginalSize = oldSize;
			ResultSize = new Size(
				(int)(oldSize.Width * Math.Abs(Math.Cos(Angle)) + oldSize.Height * Math.Abs(Math.Sin(Angle))),
				(int)(oldSize.Height * Math.Abs(Math.Cos(Angle)) + oldSize.Width * Math.Abs(Math.Sin(Angle))));
		}

		public Point? MapPoint(Point oldPoint)
		{
			oldPoint = new Point(oldPoint.X - ResultSize.Width / 2, oldPoint.Y - ResultSize.Height / 2);
			var x = OriginalSize.Width / 2 + (int)(oldPoint.X * Math.Cos(Angle) + oldPoint.Y * Math.Sin(Angle));
			var y = OriginalSize.Height / 2 + (int)(-oldPoint.X * Math.Sin(Angle) + oldPoint.Y * Math.Cos(Angle));
			if (x < 0 || x >= OriginalSize.Width || y < 0 || y >= OriginalSize.Height) return null;
			return new Point(x, y);
		}
	}
}