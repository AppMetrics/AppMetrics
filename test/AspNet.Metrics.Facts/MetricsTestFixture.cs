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
            TestContext.ResetMetricsValues();
            MetricsConfig = new MetricsConfig(TestContext);

            Server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting(options => { options.LowercaseUrls = true; });
                    services.AddMvc(options => { options.Filters.Add(new MetricsResourceFilter(new DefaultRouteTemplateResolver())); });
                    services.AddMetrics();
                })
                .Configure(app =>
                {
                    app.UseMetrics(MetricsConfig);
                    app.UseMvc();
                }));

            Client = Server.CreateClient();
        }

        public HttpClient Client { get; set; }

        public MetricsConfig MetricsConfig { get; set; }

        public TestServer Server { get; set; }

        public TestContext TestContext { get; set; }

        public void Dispose()
        {
            TestContext.ResetMetricsValues();
            TestContext.Dispose();
            MetricsConfig.Dispose();
            Client.Dispose();
            Server.Dispose();
        }
    }
}