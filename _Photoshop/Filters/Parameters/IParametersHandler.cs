namespace MyPhotoshop
{
    public interface IParametersHandler<TParams> where TParams : IParameters, new()
    {
        ParameterInfo[] GetDesсription();
        TParams CreateParams(double[] values);
    }
}
