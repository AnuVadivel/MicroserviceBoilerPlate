using MediatR;
using Payment.Api.Operation.Response;

namespace Payment.Api.Operation.Query
{
    public class BankByIdQuery : IRequest<BankResponse>
    {
        public int BankId { get; set; }
    }
}