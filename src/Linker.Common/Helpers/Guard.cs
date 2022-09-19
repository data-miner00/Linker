namespace Linker.Common.Helpers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A utility class that provides argument checking.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Checks that throws <see cref="ArgumentNullException"/> when the provided object is null.
        /// </summary>
        /// <typeparam name="T">Any class object.</typeparam>
        /// <param name="obj">The object to be checked.</param>
        /// <param name="paramName">The name of the object.</param>
        /// <returns>The object itself.</returns>
        public static T ThrowIfNull<T>(T obj, string paramName)
            where T : class
        {
            if (obj is null)
            {
                throw new ArgumentNullException(paramName, FormattableString.Invariant($"The object {paramName} cannot be null."));
            }

            return obj;
        }

        /// <summary>
        /// Checks that throws <see cref="ArgumentException"/> when the provided string is null or empty.
        /// </summary>
        /// <param name="str">The string to be checked.</param>
        /// <param name="paramName">The name of the string.</param>
        /// <returns>The string itself.</returns>
        public static string ThrowIfNullOrWhitespace(string str, string paramName)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException(paramName, FormattableString.Invariant($"The string {paramName} cannot be null or whitespace"));
            }

            return str;
        }

        /// <summary>
        /// Checks that throws <see cref="ValidationException"/> when the attribute failed to comply to the rules.
        /// </summary>
        /// <typeparam name="T">Any class object.</typeparam>
        /// <param name="obj">The object to be checked.</param>
        public static void ThrowIfValidationFailed<T>(T obj)
            where T : class
        {
            var context = new ValidationContext(obj);
            Validator.ValidateObject(obj, context, true);
        }
    }
}
