using System.Diagnostics.CodeAnalysis;

namespace Payment.Framework.Shared.Exception
{
    [ExcludeFromCodeCoverage]
    public class ConflictException : System.Exception
    {
        public ConflictException(string message, System.Exception ex = null) : base(message, ex)
        {
        }
    }
}