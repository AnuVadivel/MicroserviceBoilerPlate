using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.ConfigServer;
using VaultSharp;
using VaultSharp.V1.AuthMethods.AppRole;
using Microsoft.Extensions.Hosting;
using Payment.Api.Constant;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Network;

namespace Payment.Api.Extension
{
    [ExcludeFromCodeCoverage]
    internal static class ApiConfigurationExtensions
    {
        /// <summary>
        /// Add configuration server as config provider. When we read configs, priority order highest to lowest
        /// config server --> env variable --> app-setting files, which means, if a key-value exists in both
        /// app-settings and in config-server, config-server will take precedence.     
        /// </summary>
        /// <param name="configBuilder"></param>
        /// <param name="configRoot"></param>
        public static void AddApiConfigServer(this IConfigurationBuilder configBuilder, IConfigurationRoot configRoot)
        {
            if (!configRoot.GetValue<bool>("Spring:Enabled")) return;

            configBuilder.AddConfigServer(new ConfigServerClientSettings
            {
                Username = configRoot.GetValue<string>("Spring:Cloud:Config:Username"),
                Password = configRoot.GetValue<string>("Spring:Cloud:Config:Password"),
                Uri = configRoot.GetValue<string>("Spring:Cloud:Config:Uri"),
                Name = configRoot.GetValue<string>("Spring:Application:Name"),
                Environment = configRoot.GetValue<string>("ASPNETCORE_ENVIRONMENT")
            });
        }
        

        /// <summary>
        /// Configure log provider behaviour here, and when we inject ILogger in classes,these configuration
        /// will auto apply everywhere on all logger instances   
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder AddApiLogger(this IHostBuilder builder)
        {
            builder.ConfigureLogging((hostingContext, logging) =>
            {
                var environment = hostingContext.Configuration["ASPNETCORE_ENVIRONMENT"];
                var logConfiguration = new LoggerConfiguration()
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithMachineName()
                    .Enrich.WithCorrelationIdHeader(AppConstants.CorrelationIdHeaderName)
                    .Enrich.WithProperty("Environment", environment)
                    .WriteTo.Console();
                if (hostingContext.Configuration.GetValue<bool>("LogstashConfiguration:Enabled"))
                    logConfiguration.WriteTo.TCPSink(hostingContext.Configuration["LogstashConfiguration:Uri"],
                        new RenderedCompactJsonFormatter());
                Log.Logger = logConfiguration.CreateLogger();
            });
            return builder.UseSerilog();
        }

        public static bool IsLocalComponentTestsRunning(this IConfiguration configuration) =>
            configuration.GetValue("IsLocalComponentTests", false);
    }
}