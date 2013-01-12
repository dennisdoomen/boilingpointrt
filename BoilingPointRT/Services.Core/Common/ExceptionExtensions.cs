using System;
using System.Reflection;

namespace BoilingPointRT.Services.Common
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns the inner exception which is wrapped by the <see cref="TargetInvocationException"/> while preserving the stack trace.
        /// </summary>
        /// <param name="ex">The TargetInvocationException which is thrown.</param>
        /// <returns>The inner exception with correct stack trace.</returns>
        public static Exception Unwrap(this Exception ex)
        {
            while (ex is TargetInvocationException)
            {
                FieldInfo remoteStackTraceString =
                    typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);

                remoteStackTraceString.SetValue(ex.InnerException, ex.InnerException.StackTrace + Environment.NewLine);

                ex = ex.InnerException;
            }

            return ex;
        }

        /// <summary>
        /// Returns the first nested exception of the specified type, if any.
        /// </summary>
        /// <typeparam name="TException">The type of the nested exception to find.</typeparam>
        /// <param name="exception">The exception to inspect.</param>
        /// <returns>
        /// The nested exception, or <c>null</c> when not found.
        /// </returns>
        public static Exception FindNestedOfType<TException>(this Exception exception)
            where TException : Exception
        {
            Exception innerException = exception;
            while (innerException != null)
            {
                innerException = innerException.InnerException;
                if (innerException != null && typeof(TException).IsAssignableFrom(innerException.GetType()))
                {
                    return innerException;
                }
            }
            return null;
        }
    }
}