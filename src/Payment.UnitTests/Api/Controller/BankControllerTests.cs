using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Payment.Api.Controller;
using Payment.Api.Operation.Query;
using Payment.Api.Operation.Response;
using Payment.Tests.Common.TestData;
using Xunit;

namespace Payment.UnitTests.Api.Controller
{
    public class BankControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly BankController _controller;

        public BankControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new BankController(_mockMediator.Object);
        }

        [Fact]
        public async Task GetBankReturnsBankResponse()
        {
            var response = BankData.BankResponseFaker.Generate();
            _mockMediator.Setup(x => x.Send(It.Is<BankByIdQuery>(x => x.BankId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var results = await _controller.Bank(1);

            results.Should().BeEquivalentTo(new OkObjectResult(response));
        }

        [Fact]
        public async Task CreateBankReturnsNewBankId()
        {
            var command = BankData.BankCommandFaker.Generate();
            var response = new BankCreatedResponse() { BankId = 1 };
            _mockMediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var results = await _controller.CreateBank(command);

            results.Should().BeEquivalentTo(new ObjectResult(response) { StatusCode = StatusCodes.Status201Created });
        }
    }
}