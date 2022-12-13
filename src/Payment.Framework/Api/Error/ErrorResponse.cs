using Payment.Framework.Shared.Util;

namespace Payment.Framework.Api.Error
{
    /// <summary>
    /// A defined error response structure from the API  
    /// </summary>
    public class ErrorResponse
    {
        public ErrorDescription Error { get; set; }

        public override string ToString()
        {
            return NewtonSoftSerializeUtility.Serialize(this);
        }
    }
}