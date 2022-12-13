using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Payment.Domain.Core;
using Payment.Persistence.Repository;
using Xunit;

namespace Payment.UnitTests.Persistence.Repository
{
    [Collection("InMemDB")]
    public class BankRepositoryTests
    {
        private readonly BankRepository _repository;
        private readonly Mock<ILogger<BankRepository>> _logger;
        private readonly Mock<IMapper> _mapper;

        public BankRepositoryTests(InMemoryDbContextFixture fixture)
        {
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<BankRepository>>();
            _repository = new BankRepository(fixture.Context, _mapper.Object);
        }


        [Fact]
        public async Task ShouldGetBankDomainForGivenId()
        {
            var result = await _repository.Get(1);

            result.Id.Should().Be(1);
            result.Name.Should().Be("Bank1");
            result.IfscCode.Should().Be("IfscCode-a");
        }

        [Fact]
        public async Task ShouldThrowExceptionIfDomainForGivenIdNotExist()
        {
            await _repository.Invoking(x => x.Get(9999))
                .Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task ShouldCreateABank()
        {
            // we can create domain objects through faker as well
            var bank = new Bank.Builder()
                .WithIfscCode("IfscCode-d")
                .WithName("Bank4")
                .Build();
            var dao = new Payment.Persistence.DAO.Bank() { Name = bank.Name, IfscCode = bank.IfscCode };
            _mapper.Setup(x => x.Map<Payment.Persistence.DAO.Bank>(bank)).Returns(dao);

            var result = await _repository.Create(bank);

            result.Should().BeGreaterThan(0);
        }
    }
}