using System.Collections.Generic;
using Payment.Persistence;
using Payment.Persistence.DAO;
using Payment.Tests.Common.TestData;

namespace Payment.UnitTests.Persistence
{
    public class PaymentDbInitializer
    {
        public static void Initialize(PaymentDbContext context)
        {
            SeedBanks(context);
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