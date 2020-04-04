using MyPhotoshop;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Profiler
{
    class Program
    {
        static void Test(Func<double[], LighteningParameters> func, int n)
        {
            var values = new double[] { 1 };
            func(values);

            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < n; i++)
                func(values);
            sw.Stop();
            Console.WriteLine($"\t {sw.ElapsedMilliseconds}\tms");
        }

        static void Main()
        {
            var paramsHanldes = new SimpleParametersHandler<LighteningParameters>();
            var paramsStaticHanldes = new StaticParametersHandler<LighteningParameters>();
            var paramsExpressionHanldes = new ExpressionParametersHandler<LighteningParameters>();
            int n = 300000;
            //int n = 3000;

            Console.Write("Reflection time:");
            Test(vals => paramsHanldes.CreateParams(vals), n);

            Console.Write("Reflection static time:");
            Test(vals => paramsStaticHanldes.CreateParams(vals), n);

            Console.Write("Reflection exp time:");
            Test(vals => paramsExpressionHanldes.CreateParams(vals), n);

            Console.Write("Original time:\t");
            Test(vals => new LighteningParameters() { Coefficient = vals[0] }, n);
        }
    }
}