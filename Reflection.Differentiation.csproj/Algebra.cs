using System;
using System.Linq.Expressions;

namespace Reflection.Differentiation
{
    public class Algebra
    {
        public static Expression Differentiate(Expression body)
        {
            if (body is ConstantExpression)
                return Expression.Constant(0d);
            else if (body is ParameterExpression)
                return Expression.Constant(1d);
            else if (body is BinaryExpression op)
            {
                if (body.NodeType is ExpressionType.Add)
                    return Expression.Add(
                        Differentiate(op.Left), Differentiate(op.Right)
                        );
                else if (body.NodeType is ExpressionType.Multiply)
                    return Expression.Add(
                        Expression.Multiply(Differentiate(op.Left), op.Right),
                        Expression.Multiply(op.Left, Differentiate(op.Right))
                        );
            }
            else if (body is MethodCallExpression)
            {
                var method = body as MethodCallExpression;
                var arg = method.Arguments[0];
                var methodName = method.Method.Name;
                var diffRevert = body;
                
                if (methodName == "Sin")
                    diffRevert = Expression.Call(typeof(Math).GetMethod("Cos"), arg);
                else if (methodName == "Cos")
                    diffRevert = Expression.Negate(
                        Expression.Call(typeof(Math).GetMethod("Sin"), arg));
                return Expression.Multiply(diffRevert, Differentiate(arg));
            }
            throw new NotImplementedException("Other diffs not implemented");
        }

        public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> function) =>
            Expression.Lambda<Func<double, double>>(Differentiate(function.Body), function.Parameters);
    }
}