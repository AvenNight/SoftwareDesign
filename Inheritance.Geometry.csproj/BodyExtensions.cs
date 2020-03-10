namespace Inheritance.Geometry
{
    public static class BodyExtensions
    {
        public static Dimensions GetDimensions(this Body body)
        {
            var visitor = new DimensionsVisitor();

            body.Accept(visitor);
            return visitor.Dimensions;
        }

        public static double GetSurfaceArea(this Body body)
        {
            var visitor = new SurfaceAreaVisitor();
            
            body.Accept(visitor);
            return visitor.SurfaceArea;
        }
    }
}