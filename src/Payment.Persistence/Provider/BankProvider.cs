using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payment.Persistence.DAO;
using Payment.Persistence.Provider.Contract;

namespace Payment.Persistence.Provider
{
    /// <summary>
    /// Provider returns DAO and will be mainly used by queries handlers (read operations)
    /// These classes will wrap abstraction over DB context
    /// </summary>
    public class BankProvider : IBankProvider
    {
        private readonly PaymentDbContext _dbContext;

        public BankProvider(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Bank> Bank(int bankId) =>
            await _dbContext.Banks.FirstOrDefaultAsync(x => x.Id == bankId);

        public async Task<Bank> Bank(string code) =>
            await _dbContext.Banks.FirstOrDefaultAsync(x => x.IfscCode == code);

        public async Task<List<Bank>> Banks() =>
            await _dbContext.Banks.ToListAsync();
    }
}