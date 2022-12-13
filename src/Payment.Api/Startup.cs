using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Api.Extension;
using Payment.Persistence;

namespace Payment.Api
{
    /// <summary>
    /// Start will have all app level set up. Try to have this file as thin and readable as possible and move
    /// components in required extension configuration methods 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public const string HealthEndPoint = "/health";
        public const string AppName = "Payment API";
        public const string AppVersion = "v1";
        private static IConfiguration Configuration { get; set; }
        private IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            HostingEnvironment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
                .AddNewtonsoftJson(options => { options.AllowInputFormatterExceptionMessages = false; });

            services
                .AddDependencies(Configuration)
                .AddMemoryCache()
                .AddTelemetry(Configuration)
                .AddApiDocumentGeneration(Configuration)
                .AddApiHealthChecks(Configuration)
                .AddMapper()
                .AddValidatorsFromAssembly(typeof(Startup).Assembly);
           
            AddMvcServices(services);
            AddDbContexts(services);
            AddApiAuthentication(services);
        }

        /// <summary>
        /// This method is defined as virtual so that we can override default behaviour
        /// in component tests if required to change default auth implementation  
        /// </summary>
        /// <param name="services"></param>
        protected virtual void AddApiAuthentication(IServiceCollection services) =>
            services.AddApiAuthentication(Configuration);

        /// <summary>
        /// This method is defined as virtual so that we can override default behaviour
        /// in component tests if required to change default db context  
        /// </summary>
        /// <param name="services"></param>
        protected virtual void AddDbContexts(IServiceCollection services) =>
            services
                .AddDbContext<PaymentDbContext>(opt =>
                    opt.UseNpgsql(Configuration.GetConnectionString("Postgres")));

        /// <summary>
        /// This method is defined as virtual so that we can override default behaviour
        /// in component tests if required to change default MVC services set up
        /// </summary>
        /// <param name="services"></param>
        protected virtual void AddMvcServices(IServiceCollection services) =>
            services.AddMvcServices(Configuration);

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
                .AddRequestLogging(Configuration)
                .UseHealthChecks(HealthEndPoint)
                .UseHttpsRedirection()
                .UseStaticFiles()
                .AddApiSecurityResponseHeaders()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .AddApiMetrics(Configuration)
                .AddApiDocumentation(Configuration)
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapHealthChecks(HealthEndPoint);
                });
        }
    }
}