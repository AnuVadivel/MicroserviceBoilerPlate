using MediatR;
using Payment.Api.Operation.Response;

namespace Payment.Api.Operation.Command
{
    public class CreateBankCommand : IRequest<BankCreatedResponse>
    {
        public string IfscCode { get; set; }
        public string Name { get; set; }
    }
}