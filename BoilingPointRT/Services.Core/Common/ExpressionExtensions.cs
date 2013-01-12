using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BoilingPointRT.Services.Common
{
    public static class ExpressionExtensions
    {
        /// <summary>
        ///     Converts an expression into a <see cref = "MemberInfo" />.
        /// </summary>
        /// <param name = "expression">The expression to convert.</param>
        /// <returns>The member info.</returns>
        public static MemberInfo GetMemberInfo(this Expression expression)
        {
            var lambda = (LambdaExpression)expression;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            return memberExpression.Member;
        }

        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(
            this Expression<Func<TSource, TProperty>> propertyExpression)
        {
            MemberExpression memberExpression;

            var unaryExpression = propertyExpression.Body as UnaryExpression;
            if ((unaryExpression != null) && (unaryExpression.NodeType == ExpressionType.Convert))
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = propertyExpression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.", propertyExpression));
            }

            var propInfo = memberExpression.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.", propertyExpression));
            }

            return propInfo;
        }

        public static IEnumerable<string> GetPropertyPaths<T, TResult>(params Expression<Func<T, TResult>>[] expressions)
        {
            return expressions.Select(GetPropertyPath);
        }

        public static string GetPropertyPath<T, TResult>(this Expression<Func<T, TResult>> expression)
        {
            return String.Join(".",
                GetMembersOnPath(expression.Body as MemberExpression).Select(m => m.Member.Name).Reverse().ToArray());
        }

        private static IEnumerable<MemberExpression> GetMembersOnPath(MemberExpression expression)
        {
            while (expression != null)
            {
                yield return expression;
                expression = expression.Expression as MemberExpression;
            }
        }
    }
}