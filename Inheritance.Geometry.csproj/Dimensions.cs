namespace Inheritance.Geometry
{
    public class Dimensions
    {
        public double Width { get; }
        public double Height { get; }

        public Dimensions(double width, double height)
        {
            Width = width;
            Height = height;
        }

        protected bool Equals(Dimensions other) => Width.Equals(other.Width) && Height.Equals(other.Height);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Dimensions)obj);
        }

        public override int GetHashCode() => (Width.GetHashCode() * 397) ^ Height.GetHashCode();

        public static bool operator ==(Dimensions left, Dimensions right) => Equals(left, right);
        public static bool operator !=(Dimensions left, Dimensions right) => !Equals(left, right);
    }
}