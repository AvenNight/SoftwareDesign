//using System.Linq;

//namespace MyPhotoshop
//{
//    public static class ParametersExtensions
//    {
//        //public static ParameterInfo[] GetDesсription(this IParameters parameters)
//        //{
//        //    var r1 = parameters
//        //        .GetType();
//        //    var r2 = r1
//        //        .GetProperties();
//        //    var r3 = r2
//        //        .Select(p => p.GetCustomAttributes(typeof(ParameterInfo), false));
//        //    var r4 = r3
//        //        .Where(p => p.Length > 0);
//        //    var r5 = r4
//        //        .Select(p => p[0]);
//        //    var r6 = r5
//        //        .Cast<ParameterInfo>();
//        //    var r7 = r6
//        //        .ToArray();
//        //    return r7;
//        //}

//        public static ParameterInfo[] GetDesсription(this IParameters parameters)
//        {
//            return parameters
//                .GetType()
//                .GetProperties()
//                .Select(p => p.GetCustomAttributes(typeof(ParameterInfo), false))
//                .Where(p => p.Length > 0)
//                .Select(p => p[0])
//                .Cast<ParameterInfo>()
//                .ToArray();
//        }

//        public static void Parse(this IParameters parameters, double[] values)
//        {
//            var properties = parameters
//                .GetType()
//                .GetProperties()
//                .Where(p => p.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
//                .ToArray();

//            for (int i = 0; i < values.Length; i++)
//                properties[i].SetValue(parameters, values[i], new object[0]);
//        }
//    }
//}