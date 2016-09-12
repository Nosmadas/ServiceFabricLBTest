using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.AspNetCore.Gateway;
using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;

namespace GatewayService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder();
            builder.AddApplicationInsightsSettings(instrumentationKey: "ca4d1d70-d7e6-481c-b9cc-d3aca9fae49f");
            services.AddApplicationInsightsTelemetry(builder.Build());
            services.AddDefaultHttpRequestDispatcherProvider();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var serviceOneOptions = new GatewayOptions
            {
                ServiceUri = new Uri("fabric:/SFLB/ServiceOne", UriKind.Absolute),
                OperationRetrySettings = new OperationRetrySettings(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2), 10)
            };

            app.UseApplicationInsightsRequestTelemetry();
            app.UseApplicationInsightsExceptionTelemetry();

            app.AddRequestLogging();

            app.Map("/one", subApp =>
            {
                subApp.RunGateway(serviceOneOptions);
            });

            var serviceTwoOptions = new GatewayOptions
            {
                ServiceUri = new Uri("fabric:/SFLB/ServiceTwo", UriKind.Absolute),
                OperationRetrySettings = new OperationRetrySettings(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2), 10)
            };

            app.Map("/two", subApp => 
            {
                subApp.RunGateway(serviceTwoOptions);
            });

            app.Run(async context =>
            {
                if (context.Request.Path == "/")
                    await context.Response.WriteAsync("Hello World! :)");
                else
                    await context.Response.WriteAsync($"Route {context.Request.Path} not found :(");
            });
        }
    }

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            GatewayServiceEventSource.Current.RequestStart("Handling request: " + context.Request.Path);
            await _next.Invoke(context);
            GatewayServiceEventSource.Current.RequestStop("Finished handling request.");
        }
    }

    public static class RequestLoggingExtensions
    {
        public static IApplicationBuilder AddRequestLogging(this IApplicationBuilder app) => app.UseMiddleware<RequestLoggingMiddleware>();
    }
}
