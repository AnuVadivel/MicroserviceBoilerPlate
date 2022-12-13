using AutoMapper;
using FluentAssertions;
using Payment.Api.Operation.Mapper;
using Payment.Api.Operation.Response;
using Payment.Persistence.DAO;
using Xunit;

namespace Payment.UnitTests.Api.Operation.Mapper
{
    public class BankProfileTests
    {
        private readonly IMapper _mapper;

        public BankProfileTests()
        {
            _mapper = InitializeAutoMapper();
        }

        [Fact]
        public void ShouldMapBankDaoToDto()
        {
            var dao = new Bank { Id = 1, Name = "icici", IfscCode = "icici123" };
            var response = _mapper.Map<BankResponse>(dao);
            response.Should().BeEquivalentTo(dao);
        }

        private static IMapper InitializeAutoMapper()
        {
            return new MapperConfiguration(mc => { mc.AddProfile(new BankMapper()); }).CreateMapper();
        }
    }
}