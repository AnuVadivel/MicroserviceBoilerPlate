using System.Collections.Generic;
using Payment.Persistence;
using Payment.Persistence.DAO;

namespace Payment.ComponentTests.SetUp
{
    /// <summary>
    /// Add any initial data set-up required for test suite. It will runs only one during all tests runs
    /// </summary>
    public class PaymentDbInitializer
    {
        private readonly PaymentDbContext _context;

        public PaymentDbInitializer(PaymentDbContext context) => 
            _context = context;

        public void Seed()
        {
            //SeedBanks(_context);
        }

        private static void SeedBanks(PaymentDbContext context)
        {
            var data = new List<Bank>
            {
                new() { Name = "Bank1", Id = 1, IfscCode = "IfscCode-a" },
                new() { Name = "Bank2", Id = 2, IfscCode = "IfscCode-b" },
                new() { Name = "Bank3", Id = 3, IfscCode = "IfscCode-c" },
            };
            context.AddRange(data);
            context.SaveChanges();
        }
    }
}