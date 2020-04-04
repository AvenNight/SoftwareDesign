namespace MyPhotoshop
{
    public abstract class ParametrizedFilter<TParams> : IFilter where TParams : IParameters, new()
    {
        protected string name;
        private IParametersHandler<TParams> paramsHandler = new ExpressionParametersHandler<TParams>();

        public abstract Photo Process(Photo photo, TParams parameters);

        public Photo Process(Photo photo, double[] values)
        {
            return Process(photo, paramsHandler.CreateParams(values));
        }
        public ParameterInfo[] GetParameters() => paramsHandler.GetDesсription();
        public override string ToString() => name;
    }
}