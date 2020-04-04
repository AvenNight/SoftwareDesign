using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MyPhotoshop
{
    public class ExpressionParametersHandler<TParams> : IParametersHandler<TParams> where TParams : IParameters, new()
    {
        private static PropertyInfo[] properties;
        private static ParameterInfo[] description;

        private static Func<double[], TParams> parser;

        static ExpressionParametersHandler()
        {
            properties = typeof(TParams)
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

            // vals => new LighteningParameters() { Coefficient = vals[0] }

            var values = Expression.Parameter(typeof(double[]), "values");

            var bindings = new List<MemberAssignment>();
            for (int i = 0; i < properties.Length; i++)
            {
                var binding = Expression.Bind(
                    properties[i],
                    Expression.ArrayAccess(
                        values,
                        Expression.Constant(i))
                    );
                bindings.Add(binding);
            }

            var body = Expression.MemberInit(
                Expression.New(typeof(TParams).GetConstructor(new Type[0])),
                bindings
                );

            var ex = Expression.Lambda<Func<double[], TParams>>(
                body,
                values
                );

            parser = ex.Compile();
        }

        public ParameterInfo[] GetDesсription() => description;

        public TParams CreateParams(double[] values)
        {
            if (properties.Length != values.Length) throw new ArgumentException();
            return parser(values);
        }
    }
}