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
        private static readonly AppMetricsOptions TestOptions = new AppMetricsOptions
        {
            DefaultSamplingType = SamplingType.Default,
            GlobalContextName = "testing",
            DisableMetrics = false,
            DisableHealthChecks = false,
            JsonSchemeVersion = JsonSchemeVersion.Version1
        };

        public MetricsTestFixture(AppMetricsOptions testOptions = null)
        {
            if (testOptions == null)
            {
                testOptions = TestOptions;
            }

            TestContext = TestContextHelper.Instance();

            TestContext.Advanced.ResetMetricsValues();

            Server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting(options => { options.LowercaseUrls = true; });
                    //TODO: AH set this to the default filter?
                    services.AddMvc(options => { options.Filters.Add(new MetricsResourceFilter(new DefaultRouteTemplateResolver())); });
                    services.AddMetrics(options =>
                    {
                        options.DefaultSamplingType = testOptions.DefaultSamplingType;
                        options.GlobalContextName = testOptions.GlobalContextName;
                        options.DisableMetrics = testOptions.DisableMetrics;
                        options.DisableHealthChecks = testOptions.DisableHealthChecks;
                        options.JsonSchemeVersion = testOptions.JsonSchemeVersion;
                    }, TestContext);
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

        public IMetricsContext TestContext { get; set; }

        public void Dispose()
        {
            TestContext.Advanced.ResetMetricsValues();
            TestContext.Dispose();
            Client.Dispose();
            Server.Dispose();
        }
    }
}