using Microsoft.AspNetCore.Hosting;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceTwo
{
    public class Program
    {
        public static string NodeName { get; private set; }
        public static bool BadMode { get; set; } = false;

        // Entry point for the application.
        public static void Main(string[] args)
        {
            ServiceRuntime.RegisterServiceAsync("ServiceTwoType", context => new WebHostingService(context, "ServiceEndpoint")).GetAwaiter().GetResult();

            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        /// A specialized stateless service for hosting ASP.NET Core web apps.
        /// </summary>
        internal sealed class WebHostingService : StatelessService, ICommunicationListener
        {
            private readonly string _endpointName;

            private IWebHost _webHost;

            public WebHostingService(StatelessServiceContext serviceContext, string endpointName)
                : base(serviceContext)
            {
                _endpointName = endpointName;
            }

            protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
            {
                return new[] { new ServiceInstanceListener(_ => this) };
            }

            void ICommunicationListener.Abort()
            {
                _webHost?.Dispose();
            }

            Task ICommunicationListener.CloseAsync(CancellationToken cancellationToken)
            {
                _webHost?.Dispose();

                return Task.FromResult(true);
            }

            Task<string> ICommunicationListener.OpenAsync(CancellationToken cancellationToken)
            {
                var context = FabricRuntime.GetActivationContext();
                var endpoint = context.GetEndpoint(_endpointName);

                var nodeContext = FabricRuntime.GetNodeContext();

                NodeName = nodeContext.NodeName;
                ServiceTwoEventSource.Current.Log($"NodeName: {NodeName}");

                //string serverUrl = $"{endpoint.Protocol}://{FabricRuntime.GetNodeContext().IPAddressOrFQDN}:{endpoint.Port}";
                string serverUrl = $"{endpoint.Protocol}://+:{endpoint.Port}/Two";

                _webHost = new WebHostBuilder().UseWebListener()
                                               .UseContentRoot(Directory.GetCurrentDirectory())
                                               .UseStartup<Startup>()
                                               .UseUrls(serverUrl)
                                               .Build();

                _webHost.Start();

                return Task.FromResult(serverUrl);
            }
        }
    }
}
