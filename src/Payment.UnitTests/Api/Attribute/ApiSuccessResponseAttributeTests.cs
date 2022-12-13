using FluentAssertions;
using Payment.Api.Attribute;
using Payment.Api.Operation.Response;
using Xunit;

namespace Payment.UnitTests.Api.Attribute
{
    public class ApiSuccessResponseAttributeTests
    {
        [Fact]
        public void ShouldSetStatusdDescriptionAndResponseType()
        {
            var attr = new ApiSuccessResponseAttribute(201, "something created", typeof(BankCreatedResponse));
            
            attr.StatusCode.Should().Be(201);
            attr.Description.Should().Be("something created");
            attr.Type.Name.Should().Be(nameof(BankCreatedResponse));
        }
    }
}