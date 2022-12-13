using FluentAssertions;
using Payment.Domain.Core;
using Xunit;

namespace Payment.UnitTests.Domain.Core
{
    public class BankBuilderTests
    {
        [Fact]
        public void ShouldConstructAValidBankDomain()
        {
            var bank = new Bank.Builder()
                .WithIfscCode("icici123")
                .WithName("icici")
                .WithBankId(1)
                .Build();

            bank.Id.Should().Be(1);
            bank.Name.Should().Be("icici");
            bank.IfscCode.Should().Be("icici123");
        }
    }
}