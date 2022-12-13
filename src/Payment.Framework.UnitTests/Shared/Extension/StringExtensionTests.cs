using System;
using FluentAssertions;
using Payment.Framework.Shared.Extension;
using Xunit;
using ArgumentException = Payment.Framework.Shared.Exception.ArgumentException;

namespace Payment.Framework.UnitTests.Shared.Extension
{
    public class StringExtensionTests
    {
        [Fact]
        public void ShouldReturnTrueIfStringIsEmptyOrNull()
        {
            ((string)null).IsNullOrWhiteSpace().Should().BeTrue();
            "".IsNullOrWhiteSpace().Should().BeTrue();
        }

        [Fact]
        public void ShouldThrowExceptionIfStringNullOrEmpty()
        {
            var message = "string can't be null";

            Func<object> func = () => ((string)null).EnsureNotNull<ArgumentException>("string can't be null");
            func.Should().ThrowExactly<ArgumentException>().WithMessage(message);

            "".Invoking(x => x.EnsureNotNullOrWhiteSpace<ArgumentException>(message))
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage(message);
        }

        [Fact]
        public void ShouldReturnFalseIfStringIsNotEmptyOrNull()
        {
            "test".IsNullOrWhiteSpace().Should().BeFalse();
        }


        [Fact]
        public void ShouldReturnCorrectOutputByEqualIgnoreCulture()
        {
            "test".EqualIgnoreCulture("test1").Should().BeFalse();
            "test".EqualIgnoreCulture("test").Should().BeTrue();
            "test".EqualIgnoreCulture("TEST").Should().BeTrue();
            "test".EqualIgnoreCulture("Test").Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnValueWithEscapeSingleQuote()
        {
            "test".EscapeQuotes().Should().Be("test");
            "first'name".EscapeQuotes().Should().Be("first''name");
            "".EscapeQuotes().Should().Be("");
        }
    }
}