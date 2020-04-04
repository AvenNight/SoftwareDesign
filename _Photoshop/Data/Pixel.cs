using System;

namespace MyPhotoshop
{
	public struct Pixel
	{
		private double r, g, b;
		public double R
		{
			set => r = Check(value);
			get => r;
		}
		public double G
		{
			set => g = Check(value);
			get => g;
		}
		public double B
		{
			set => b = Check(value);
			get => b;
		}

		public Pixel(double red, double green, double blue) : this() // для инициализации r = g = b = 0;
		{
			R = red;
			G = green;
			B = blue;
		}
		public Pixel(double white) : this() // для инициализации r = g = b = 0;
		{
			R = white;
			G = white;
			B = white;
		}

		//private double Check(double value) =>
		//	value < 0 ? 0 : value > 1 ? 1 : value;	// типа как плохая практика - маскировка ошибок
		private double Check(double value)
		{
			if (value < 0 || value > 1)
				throw new ArgumentOutOfRangeException("RGB color value must be 0..1");
			return value;
		}

		public static double Trim(double value) =>
			value < 0 ? 0 : value > 1 ? 1 : value;

		public static Pixel operator *(Pixel p, double d) =>
			new Pixel(
				Trim(p.R * d),
				Trim(p.G * d),
				Trim(p.B * d));
		public static Pixel operator *(double d, Pixel p) =>
			p * d;
		public static Pixel operator /(Pixel p, double d) =>
			new Pixel(p.R / d, p.G / d, p.B / d);
	}
}