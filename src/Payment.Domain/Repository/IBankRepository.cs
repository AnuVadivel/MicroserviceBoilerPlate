using System.Threading.Tasks;

namespace Payment.Domain.Repository
{
    /// <summary>
    /// In DDD, domain expose contract how domain will be persisted and that's why these contracts are defined in domain
    /// and will be implemented by actual repositories in Persistence layer 
    /// </summary>
    public interface IBankRepository
    {
        Task<Core.Bank> Get(int bankId);
        Task<int> Create(Core.Bank bank);
    }
}