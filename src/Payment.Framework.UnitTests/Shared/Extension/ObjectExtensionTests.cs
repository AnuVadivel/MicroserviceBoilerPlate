using System;
using FluentAssertions;
using Payment.Framework.Shared.Extension;
using Xunit;
using ArgumentException = Payment.Framework.Shared.Exception.ArgumentException;

namespace Payment.Framework.UnitTests.Shared.Extension
{
    public class ObjectExtensionTests
    {
        [Fact]
        public void ShouldThrowExceptionIfObjectIsNull()
        {
            Func<object> func = () => ((object)null).EnsureNotNull<ArgumentException>("val can't be null");
            func.Should().Throw<ArgumentException>().WithMessage("val can't be null");
        }

        [Fact]
        public void ShouldNotThrowExceptionIfObjectIsNull()
        {
            new object().Invoking(x => x.EnsureNotNull<ArgumentException>("val can't be null"))
                .Should().NotThrow<ArgumentException>();
        }
    }
}