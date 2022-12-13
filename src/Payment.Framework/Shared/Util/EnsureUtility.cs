using System;

namespace Payment.Framework.Shared.Util
{
    public static class EnsureUtility
    {
        public static void EnsureThat<TException>(bool condition, string message = "",
            System.Exception exception = null) where TException : System.Exception
        {
            if (!condition)
                throw (TException)Activator.CreateInstance(typeof(TException), message, exception);
        }
    }
}