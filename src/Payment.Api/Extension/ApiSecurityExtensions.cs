using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Payment.Api.Extension
{
    [ExcludeFromCodeCoverage]
    internal static class ApiSecurityExtensions
    {
        /// <summary>
        /// We can change this method to add/update desired auth policy for the application (Okta, AzureAd, AzureApp,  etc....)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.IsLocalComponentTestsRunning())
                return services;

            if (bool.TryParse(configuration["Auth:Enabled"], out var isAuthEnabled) && isAuthEnabled)
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    // Only for development environment
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.Authority = configuration["Auth:Authority"];
                    options.Audience = configuration["Auth:Audience"];
                });
            else
                services.AddAuthorization(x =>
                {
                    x.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(_ => true)
                        .Build();
                });

            return services;
        }
    }
}