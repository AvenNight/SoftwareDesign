using System;
using System.Linq;

namespace MyPhotoshop
{
    public class SimpleParametersHandler<TParams> : IParametersHandler<TParams> where TParams : IParameters, new()
    {
        public ParameterInfo[] GetDesсription()
        {
            return typeof(TParams)
                .GetProperties()
                .Select(p => p.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(p => p.Length > 0)
                .Select(p => p[0])
                .Cast<ParameterInfo>()
                .ToArray();
        }

        public TParams CreateParams(double[] values)
        {
            var tp = new TParams();

            var properties = tp
                .GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
                .ToArray();

            if (properties.Length != values.Length) throw new ArgumentException();

            for (int i = 0; i < values.Length; i++)
                properties[i].SetValue(tp, values[i], new object[0]);

            return tp;
        }
    }
}