using FluentAssertions;
using Payment.Api.Attribute;
using Payment.Framework.Api.Error;
using Xunit;

namespace Payment.UnitTests.Api.Attribute
{
    public class ApiErrorResponseAttributeTests
    {
        [Fact]
        public void ShouldSetStatusdDescriptionAndErrorResponseType()
        {
            var attr = new ApiErrorResponseAttribute(400, "something bad");
            
            attr.StatusCode.Should().Be(400);
            attr.Description.Should().Be("something bad");
            attr.Type.Name.Should().Be(nameof(ErrorResponse));
        }
    }
    
    
}