using System.Diagnostics.CodeAnalysis;

namespace Payment.Framework.Shared.Exception
{
    [ExcludeFromCodeCoverage]
    public class ArgumentException : System.Exception
    {
        public ArgumentException(string message, System.Exception ex = null) : base(message, ex)
        {
        }
    }
}