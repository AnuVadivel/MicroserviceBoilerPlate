using System;
using Swashbuckle.AspNetCore.Annotations;

namespace Payment.Api.Attribute
{
    /// <summary>
    /// Abstraction over default swagger attrs  
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ApiSuccessResponseAttribute : SwaggerResponseAttribute
    {
        public ApiSuccessResponseAttribute(int statusCode, string description = null, Type type = null)
            : base(statusCode, description, type)
        {
        }
    }
}