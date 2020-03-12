using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Reflection.Randomness
{
    public class FromDistributionAttribute : Attribute
    {
        private readonly Type type;
        private readonly object[] @params;

        public IContinousDistribution Distribution
        {
            get
            {
                if (@params.Length > 2) throw new ArgumentException($"{type}");
                return (IContinousDistribution)Activator.CreateInstance(type, @params);
            }
        }

        public FromDistributionAttribute(Type type, params object[] ps)
        {
            this.type = type;
            @params = ps;
        }
    }

    public class Generator<T> where T : new()
    {
        public readonly static Dictionary<PropertyInfo, IContinousDistribution> Distributions;
        private static IEnumerable<PropertyInfo> properties;

        static Generator()
        {
            properties = typeof(T).GetProperties();
            Distributions = new Dictionary<PropertyInfo, IContinousDistribution>();
        }

        public T Generate(Random rnd)
        {
            T e = new T();
            foreach (var prop in properties)
            {
                var curAttr = (FromDistributionAttribute)prop.GetCustomAttribute(typeof(FromDistributionAttribute));
                IContinousDistribution rndDistrib = null;
                if (Distributions.ContainsKey(prop))
                    rndDistrib = Distributions[prop];
                else if (curAttr != null)
                    rndDistrib = curAttr.Distribution;
                else
                    continue;
                prop.SetValue(e, rndDistrib.Generate(rnd));
            }
            return e;
        }
    }

    public static class GeneratorExtensions
    {
        public static CustomGenerator<T> For<T>(this Generator<T> generator, Expression<Func<T, double>> pointer) where T : new()
        {
            if (pointer.Body is MemberExpression memberPointer)
            {
                var pointedProperty = (PropertyInfo)memberPointer.Member;
                if (!typeof(T).GetProperties().Contains(pointedProperty)) throw new ArgumentException();
                return new CustomGenerator<T>(generator, pointedProperty);
            }
            throw new ArgumentException();
        }

        public static Generator<T> Set<T>(this CustomGenerator<T> customGenerator, IContinousDistribution newDistribution) where T : new()
        {
            Generator<T>.Distributions[customGenerator.PointedProperty] = newDistribution;
            return customGenerator.Generator;
        }

        public class CustomGenerator<T> where T : new()
        {
            public readonly Generator<T> Generator;
            public readonly PropertyInfo PointedProperty;

            public CustomGenerator(Generator<T> generator, PropertyInfo pointedProperty)
            {
                Generator = generator;
                PointedProperty = pointedProperty;
            }
        }
    }
}