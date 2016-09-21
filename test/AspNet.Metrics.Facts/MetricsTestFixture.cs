using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using AspNet.Metrics.Infrastructure;
using Metrics;
using Microsoft.AspNet.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Metrics.Facts
{
    public class MetricsTestFixture : IDisposable
    {
        public MetricsTestFixture()
        {
            TestContext = new TestContext();
            MetricsConfig = new MetricsConfig(TestContext);

            Server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting(options => { options.LowercaseUrls = true; });
                    services.AddMvc(options => { options.Filters.Add(new MetricsResourceFilter(new DefaultRouteTemplateResolver())); });
                    services.AddMetrics();
                    services.Add(new ServiceDescriptor(TestContext.GetType(), s => TestContext, ServiceLifetime.Transient));
                })
                .Configure(app =>
                {
                    app.UseMetrics(MetricsConfig);
                    app.UseMvc();
                }));

            Client = Server.CreateClient();
        }

        public HttpClient Client { get; }

        public MetricsConfig MetricsConfig { get; }

        public TestServer Server { get; }

        public TestContext TestContext { get; }

        public void Dispose()
        {
            Server.Dispose();
            Client.Dispose();
            TestContext.Dispose();
            MetricsConfig.Dispose();
        }
    }
}