using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Prometheus;
using Serilog;

namespace Payment.Api.Extension
{
    [ExcludeFromCodeCoverage]
    internal static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Few default security header added as middleware
        /// we can update this list as per project needs
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddApiSecurityResponseHeaders(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Cache-Control", "no-store");
                context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'none'");
                context.Response.Headers.Add("Content-Type", "application/json");
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000 ; includeSubDomains");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

                await next();
            });
        }

        /// <summary>
        /// Default Prometheus metrics exporter
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddApiMetrics(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (configuration.IsLocalComponentTestsRunning())
                return app;

            return app
                .UseMetricServer()
                .UseHttpMetrics();
        }

        /// <summary>
        /// Adds middleware for streamlined request logging. Instead of writing HTTP request information
        /// like method, path, timing, status code and exception details
        /// in several events, this middleware collects information during the request (including from
        /// <see cref="T:Serilog.IDiagnosticContext" />), and writes a single event at request completion.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddRequestLogging(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (configuration.IsLocalComponentTestsRunning())
                return app;

            return app
                .UseSerilogRequestLogging();
        }

        /// <summary>
        /// Add Swagger documentation. It also support authorization to your oAuth server.
        /// Configure app-settings with oAuth server required details to provide values in 
        /// OAuthClientId, OAuthClientSecret, OAuthRealm
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddApiDocumentation(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (configuration.IsLocalComponentTestsRunning())
                return app;

            return app
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", Startup.AppName + Startup.AppVersion);
                    c.OAuthClientId(configuration["Auth:OAuthClientId"]);
                    c.OAuthClientSecret(configuration["Auth:OAuthClientSecret"]);
                    c.OAuthRealm(configuration["Auth:OAuthRealm"]);
                    c.OAuthAppName(Startup.AppName);
                    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                })
                .UseStaticFiles();
        }
    }
}