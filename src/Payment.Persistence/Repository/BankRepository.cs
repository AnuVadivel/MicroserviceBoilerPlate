using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Core;
using Payment.Domain.Repository;
using Payment.Framework.Shared.Exception;
using Payment.Framework.Shared.Extension;


namespace Payment.Persistence.Repository
{
    /// <summary>
    /// Repositories will accept and return domains only, which further transpose into DAOs
    /// Repository will take care of all transformations required for domain to dao and vice versa
    /// </summary>
    public class BankRepository : IBankRepository
    {
        private readonly PaymentDbContext _dbContext;
        private readonly IMapper _mapper;

        public BankRepository(PaymentDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Bank> Get(int bankId)
        {
            var dao = await _dbContext.Banks.FirstOrDefaultAsync(x => x.Id == bankId)?? throw new NullReferenceException();
            dao.EnsureNotNull<NotFoundException>($"No Bank exist for given bank id: {bankId}");

            // builder will be used to map DAO to domain
            return new Bank.Builder()
                .WithIfscCode(dao.IfscCode)
                .WithName(dao.Name)
                .WithBankId(dao.Id)
                .Build();
        }

        public async Task<int> Create(Bank bank)
        {
            var bankDao = _mapper.Map<DAO.Bank>(bank);
            await _dbContext.Banks.AddAsync(bankDao);
            await _dbContext.SaveChangesAsync();
            return bankDao.Id;
        }
    }
}