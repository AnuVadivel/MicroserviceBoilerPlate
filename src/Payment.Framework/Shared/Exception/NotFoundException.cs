using System.Diagnostics.CodeAnalysis;

namespace Payment.Framework.Shared.Exception
{
    [ExcludeFromCodeCoverage]
    public class NotFoundException : System.Exception
    {
        public NotFoundException(string message, System.Exception ex = null) : base(message, ex)
        {
        }
    }
}