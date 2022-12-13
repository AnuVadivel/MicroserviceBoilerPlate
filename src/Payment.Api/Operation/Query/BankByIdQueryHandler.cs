using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Payment.Api.Operation.Response;
using Payment.Framework.Shared.Exception;
using Payment.Framework.Shared.Extension;
using Payment.Persistence.Provider.Contract;

namespace Payment.Api.Operation.Query
{
    public class BankByIdQueryHandler : IRequestHandler<BankByIdQuery, BankResponse>
    {
        private readonly IBankProvider _bankProvider;
        private readonly IMapper _mapper;

        public BankByIdQueryHandler(IBankProvider bankProvider, IMapper mapper)
        {
            _bankProvider = bankProvider;
            _mapper = mapper;
        }

        public async Task<BankResponse> Handle(BankByIdQuery query, CancellationToken cancellationToken)
        {
            var bank = await _bankProvider.Bank(query.BankId);
            bank.EnsureNotNull<NotFoundException>($"Bank with {query.BankId} not exist.");
            return _mapper.Map<BankResponse>(bank);
        }
    }
}