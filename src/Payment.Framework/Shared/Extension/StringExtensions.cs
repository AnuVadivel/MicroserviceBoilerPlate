using System;
using Payment.Framework.Shared.Util;

namespace Payment.Framework.Shared.Extension
{
    public static class StringExtensions
    {
        public static string EnsureNotNullOrWhiteSpace<TException>(this string value, string message = "",
            System.Exception exception = null) where TException : System.Exception
        {
            EnsureUtility.EnsureThat<TException>(!value.IsNullOrWhiteSpace(), message, exception);
            return value;
        }

        public static bool IsNullOrWhiteSpace(this string val) =>
            string.IsNullOrWhiteSpace(val);

        public static bool EqualIgnoreCulture(this string val, string target) =>
            string.Equals(val, target, StringComparison.InvariantCultureIgnoreCase);

        public static string EscapeQuotes(this string val) =>
            val?.Replace("'", "''");

        public static int ToInt(this string val) =>
            int.Parse(val);
    }
}