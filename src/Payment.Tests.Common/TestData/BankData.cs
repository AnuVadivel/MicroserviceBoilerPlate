using Bogus;
using Payment.Api.Operation.Command;
using Payment.Api.Operation.Response;

namespace Payment.Tests.Common.TestData
{
    public static class BankData
    {
        public static Faker<BankResponse> BankResponseFaker { get; } =
            new Faker<BankResponse>()
                .RuleFor(x => x.Id, 1)
                .RuleFor(x => x.Name, f => f.Name.FindName())
                .RuleFor(x => x.IfscCode, f => f.Random.String(6,6));
    
        public static Faker<CreateBankCommand> BankCommandFaker { get; } =
            new Faker<CreateBankCommand>()
                .RuleFor(x => x.Name, f => f.Name.FindName())
                .RuleFor(x => x.IfscCode, f => f.Random.String(6,6));
    }
}