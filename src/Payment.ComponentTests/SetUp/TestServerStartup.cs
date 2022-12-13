using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Api;
using Payment.Framework.Api.Filter;
using Payment.Persistence;

namespace Payment.ComponentTests.SetUp
{
    /// <summary>
    /// We can override default behaviour of actual api start up class in case we need to set up something custom 
    /// </summary>
    public class TestServerStartup : Startup, IDisposable
    {
        public TestServerStartup(IConfiguration configuration, IWebHostEnvironment env)
            : base(configuration, env)
        {
        }

        protected override void AddApiAuthentication(IServiceCollection services)
        {
        }

        protected override void AddDbContexts(IServiceCollection services)
        {
            services.AddDbContext<PaymentDbContext>(
                options => options.UseInMemoryDatabase("PaymentDb"));
        }

        protected override void AddMvcServices(IServiceCollection services)
        {
            services.AddMvcCore(options =>
                {
                    options.Filters.Add(typeof(PaymentExceptionFilterAttribute));
                    options.Filters.Add<ModelValidationFilter>();
                })
                .AddApplicationPart(typeof(Startup).Assembly);
        }

        public void Dispose()
        {
        }
    }
}