using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Payment.Api.Operation.Command;
using Payment.Api.Operation.Response;
using Payment.Domain.Core;
using Payment.Domain.Repository;
using Payment.Tests.Common.TestData;
using Xunit;

namespace Payment.UnitTests.Api.Operation.Command
{
    public class CreateBankCommandHandlerTests
    {
        private readonly Mock<IBankRepository> _repository;
        private readonly CreateBankCommandHandler _handler;

        public CreateBankCommandHandlerTests()
        {
            _repository = new Mock<IBankRepository>();
            _handler = new CreateBankCommandHandler(_repository.Object);
        }

        [Fact]
        public async Task ShouldCreateBank()
        {
            var command = BankData.BankCommandFaker.Generate();
            _repository.Setup(x => x.Create(It.Is<Bank>(x => x.IfscCode == command.IfscCode && x.Name == command.Name)))
                .ReturnsAsync(1);

            var results = await _handler.Handle(command, default);

            results.Should().BeEquivalentTo(new BankCreatedResponse { BankId = 1 });
        }
    }
}