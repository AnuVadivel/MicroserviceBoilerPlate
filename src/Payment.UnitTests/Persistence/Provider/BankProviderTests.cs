using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Payment.Persistence.Provider;
using Xunit;

namespace Payment.UnitTests.Persistence.Provider
{
    [Collection("InMemDB")]
    public class BankProviderTests
    {
        private readonly BankProvider _bankProvider;

        public BankProviderTests(InMemoryDbContextFixture fixture)
        {
            _bankProvider = new BankProvider(fixture.Context);
        }

        [Fact]
        public async Task ShouldGetAllBanks()
        {
            var results = await _bankProvider.Banks();

            results.Count.Should().BeGreaterOrEqualTo(3);
            results.First(x => x.Name == "Bank1").Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldGetBankForGivenId()
        {
            var result = await _bankProvider.Bank(1);

            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task ShouldGetBankForGivenIfscCode()
        {
            var results = await _bankProvider.Bank("IfscCode-a");

            results.IfscCode.Should().Be("IfscCode-a");
        }
        
        [Fact]
        public async Task ShouldReturnNullIfGivenIfscCodeNotExist()
        {
            var results = await _bankProvider.Bank("aakshkahSH");

            results.Should().BeNull();
        }
    }
}