using FluentValidation;
using Payment.Api.Operation.Command;
using Payment.Persistence.Provider.Contract;

namespace Payment.Api.Operation.Validator
{
    public class CreateBankCommandValidator : AbstractValidator<CreateBankCommand>
    {
        public CreateBankCommandValidator(IBankProvider bankProvider)
        {
            RuleFor(x => x.IfscCode)
                .NotEmpty()
                .Must(s => bankProvider.Bank(s).Result == null).WithMessage("Bank details already exists");
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}