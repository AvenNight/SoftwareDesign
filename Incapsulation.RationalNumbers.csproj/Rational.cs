using System;

namespace Incapsulation.RationalNumbers
{
    public struct Rational
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public bool IsNan => Denominator == 0;

        public Rational(int numerator, int denominator)
        {
            if (numerator != 0 && numerator != 1 && denominator != 0)
            {
                int div = Math.Abs(numerator);
                while (div >= 1 && denominator % div != 0 || numerator % div != 0)
                    div--;

                denominator = denominator / div;
                numerator = numerator / div;
            }
            Numerator = denominator < 0 ? -numerator : numerator;
            Denominator = denominator < 0 ? -denominator : denominator;
        }

        public Rational(int numerator) : this(numerator, 1) { }

        public static implicit operator double(Rational r)
        {
            if (r.IsNan) return double.NaN;
            return (double)r.Numerator / r.Denominator;
        }
        public static implicit operator Rational(int c) =>
            new Rational(c);
        public static explicit operator int(Rational r)
        {
            if (r.Numerator != 0 && r.Numerator % r.Denominator != 0)
                throw new Exception();
            return r.Numerator / r.Denominator;
        }

        public static Rational operator *(Rational r, int c) =>
            new Rational(r.Numerator * c, r.Denominator);
        public static Rational operator *(int c, Rational r) =>
            new Rational(r.Numerator * c, r.Denominator);
        public static Rational operator *(Rational r1, Rational r2) =>
            new Rational(r1.Numerator * r2.Numerator, r1.Denominator * r2.Denominator);

        public static Rational operator /(Rational r, int c) =>
            new Rational(r.Numerator, r.Denominator * c);
        public static Rational operator /(int c, Rational r) =>
            new Rational(r.Denominator * c, r.Numerator);
        public static Rational operator /(Rational r1, Rational r2)
        {
            if (r1.IsNan || r2.IsNan) return new Rational(1, 0);
            return r1 * (1 / r2);
        }

        public static Rational operator +(Rational r, int c) =>
            new Rational(r.Numerator + r.Denominator * c, r.Denominator);
        public static Rational operator +(int c, Rational r) =>
            r + c;
        public static Rational operator +(Rational r1, Rational r2) =>
            new Rational(r1.Numerator * r2.Denominator + r2.Numerator * r1.Denominator, r1.Denominator * r2.Denominator);

        public static Rational operator -(Rational r, int c) =>
            new Rational(r.Numerator - r.Denominator * c, r.Denominator);
        public static Rational operator -(int c, Rational r) =>
            new Rational(r.Denominator * c - r.Numerator, r.Denominator);
        public static Rational operator -(Rational r1, Rational r2) =>
            new Rational(r1.Numerator * r2.Denominator - r2.Numerator * r1.Denominator, r1.Denominator * r2.Denominator);
    }
}