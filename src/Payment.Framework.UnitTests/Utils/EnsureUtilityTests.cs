using System;
using FluentAssertions;
using Payment.Framework.Shared.Exception;
using Payment.Framework.Shared.Util;
using Xunit;

namespace Payment.Framework.UnitTests.Utils
{
    public class EnsureUtilityTests
    {
        [Fact]
        public void EnsureThat_WhenConditionIsFalse_ThrowsException()
        { 
            Action action = () => EnsureUtility.EnsureThat<BadRequestException>(false, "Bad Request");
            action.Should().Throw<BadRequestException>().WithMessage("Bad Request");
        }
    }  
}

