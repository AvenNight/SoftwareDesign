using System;
using System.Linq;
using System.Reflection;

namespace MyPhotoshop
{
    public class StaticParametersHandler<TParams> : IParametersHandler<TParams> where TParams : IParameters, new()
    {
        private static TParams param;
        private static PropertyInfo[] properties;
        private static ParameterInfo[] description;

        static StaticParametersHandler()
        {
            param = new TParams();
            properties = param
                .GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
                .ToArray();
            description = typeof(TParams)
                .GetProperties()
                .Select(p => p.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(p => p.Length > 0)
                .Select(p => p[0])
                .Cast<ParameterInfo>()
                .ToArray();
        }

        public ParameterInfo[] GetDesсription()
        {
            return description;
        }

        public TParams CreateParams(double[] values)
        {
            if (properties.Length != values.Length) throw new ArgumentException();

            for (int i = 0; i < values.Length; i++)
                properties[i].SetValue(param, values[i], new object[0]);

            return param;
        }
    }
}