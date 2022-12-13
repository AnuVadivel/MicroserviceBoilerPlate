using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using AutoMapper;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Payment.Framework.Api.Filter;
using Payment.Persistence;

namespace Payment.Api.Extension
{
    [ExcludeFromCodeCoverage]
    internal static class ServiceCollectionExtensions
    {
 

        /// <summary>
        /// Telemetry adds required instrumentation with the Jeager for tracing    
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.IsLocalComponentTestsRunning())
                return services;

            // this adds jaeger as default tracing tool. We can chane code block to replace other implementations. 
            return services.AddOpenTelemetryTracing((builder) =>
            {
                builder.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("PaymentService"))
                    .AddJaegerExporter(opts =>
                    {
                        opts.AgentHost = configuration["Jaeger:AgentHost"];
                        opts.AgentPort = Convert.ToInt32(configuration["Jaeger:AgentPort"]);
                        opts.ExportProcessorType = ExportProcessorType.Simple;
                    });
            });
        }

        /// <summary>
        /// Add swagger documentation with all required oAuth flow, so that clients can directly authenticate through oAuth
        /// server and try all APIs.
        /// It also add validation rules added through fluent validation (ref : CreateBankCommandValidator)
        /// by extension : AddFluentValidationRulesToSwagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddApiDocumentGeneration(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.IsLocalComponentTestsRunning())
                return services;

            // Ref:https://github.com/domaindrivendev/Swashbuckle.AspNetCore --> To explore more on swagger integration and som other supported libs
            return services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(Startup.AppVersion, new OpenApiInfo
                    {
                        Title = Startup.AppName, Version = Startup.AppVersion,
                        Description = "Supports payment operations "
                    });
                    c.EnableAnnotations();
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                })
                .AddFluentValidationRulesToSwagger();
        }

        public static IServiceCollection AddMvcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvcCore(options =>
                { 
                    options.Filters.Add(typeof(PaymentExceptionFilterAttribute));
                    options.Filters.Add<ModelValidationFilter>();
                    if (configuration.GetValue<bool>("Auth:Enabled"))
                        options.Filters.Add(new AuthorizeFilter());
                })
                .AddApiExplorer();
            return services;
        }

        /// <summary>
        /// Add AutoMapper for all assemblies 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddMaps(
                    "Payment.Api",
                    "Payment.Domain",
                    "Payment.Persistence"
                );
            });

            // Uncomment below line to enable runtime validation for all mappings definitions across assemblies 
            // mappingConfig.AssertConfigurationIsValid();

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        /// <summary>
        /// Add health check url with all required checks. We can enhance this checks as per the need   
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddApiHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<PaymentDbContext>();
            return services;
        }
    }
}