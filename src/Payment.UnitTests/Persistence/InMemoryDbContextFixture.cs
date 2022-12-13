using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Payment.Persistence;

namespace Payment.UnitTests.Persistence
{
    public class InMemoryDbContextFixture : IDisposable
    {
        public IMapper Mapper { get; }
        public PaymentDbContext Context { get; }

        public InMemoryDbContextFixture()
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddMaps("Payment.Persistence"); });

            Mapper = mappingConfig.CreateMapper();

            var options = new DbContextOptionsBuilder<PaymentDbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            Context = new PaymentDbContext(options);
            Context.Database.EnsureCreated();
            PaymentDbInitializer.Initialize(Context);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
                Context.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}