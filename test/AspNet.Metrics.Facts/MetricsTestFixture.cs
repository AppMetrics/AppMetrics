using System;
using System.Net.Http;
using App.Metrics;
using App.Metrics.Json;
using AspNet.Metrics.Infrastructure;
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

            Server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting(options => { options.LowercaseUrls = true; });
                    //TODO: AH set this to the default filter?
                    services.AddMvc(options => { options.Filters.Add(new MetricsResourceFilter(new DefaultRouteTemplateResolver())); });
                    services.AddMetrics(options =>
                    {
                        //TODO: Test different app metrics options
                        options.DefaultSamplingType = SamplingType.Default;
                        options.GlobalContextName = "testing";
                        options.DisableMetrics = false;
                        options.MetricsContext = TestContext;
                        options.JsonSchemeVersion = JsonSchemeVersion.Version1;
                    });
                })
                .Configure(app =>
                {
                    //TODO: AH - test different aspnet metrics options
                    app.UseMetrics();
                    app.UseMvc();
                }));

            Client = Server.CreateClient();
        }

        public HttpClient Client { get; set; }

        public TestServer Server { get; set; }

        public TestContext TestContext { get; set; }

        public void Dispose()
        {
            TestContext.ResetMetricsValues();
            TestContext.Dispose();
            Client.Dispose();
            Server.Dispose();
        }
    }
}