namespace MyPhotoshop
{
    public class GrayscaleParameters : IParameters
    {
        [ParameterInfo(Name = "Коэффициент", MaxValue = 1, MinValue = 0, Increment = 0.1, DefaultValue = 1)]
        public double Coefficient { get; private set; }
    }
}