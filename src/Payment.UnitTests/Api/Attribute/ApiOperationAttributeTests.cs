using FluentAssertions;
using Payment.Api.Attribute;
using Xunit;

namespace Payment.UnitTests.Api.Attribute
{
    public class ApiOperationAttributeTests
    {
        [Fact]
        public void ShouldSetSummaryAndDescription()
        {
            var attr = new ApiOperationAttribute("summary", "desc");
            
            attr.Summary.Should().Be("summary");
            attr.Description.Should().Be("desc");
        }
    }
}