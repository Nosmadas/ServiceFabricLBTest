using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ServiceTwo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder();
            builder.AddApplicationInsightsSettings(instrumentationKey: "ca4d1d70-d7e6-481c-b9cc-d3aca9fae49f");
            services.AddApplicationInsightsTelemetry(builder.Build());
            services.AddMvcCore();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseApplicationInsightsRequestTelemetry();
            app.UseApplicationInsightsExceptionTelemetry();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.AddRequestLogging();
            app.UseMvc();

            app.Run(async (context) =>
            {
                var message = $"Hello World. Love {Program.NodeName} running ServiceTwo :)";
                ServiceTwoEventSource.Current.Log($"Writing {message}");
                await context.Response.WriteAsync(message);
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
            ServiceTwoEventSource.Current.RequestStart("Handling request: " + context.Request.Path);
            await _next.Invoke(context);
            ServiceTwoEventSource.Current.RequestStop("Finished handling request.");
        }
    }

    public static class RequestLoggingExtensions
    {
        public static IApplicationBuilder AddRequestLogging(this IApplicationBuilder app) => app.UseMiddleware<RequestLoggingMiddleware>();
    }
}
