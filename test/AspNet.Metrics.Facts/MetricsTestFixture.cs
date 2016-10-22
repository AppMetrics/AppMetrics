using System;
using System.Net.Http;
using App.Metrics;
using App.Metrics.Json;
using Microsoft.AspNet.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Metrics.Facts
{
    public class MetricsTestFixture : IDisposable
    {
        private static readonly AppMetricsOptions TestOptions = new AppMetricsOptions
        {
            DefaultGroupName = "testing",
            DisableMetrics = false,
            DisableHealthChecks = false,
            JsonSchemeVersion = JsonSchemeVersion.Version1
        };

        private static readonly AspNetMetricsOptions TestAspNetOptions = new AspNetMetricsOptions
        {
            MetricsTextEnabled = true,
            HealthEnabled = true,
            MetricsEnabled = true,
            PingEnabled = true
        };

        public MetricsTestFixture(AppMetricsOptions testOptions = null,
            AspNetMetricsOptions testAspNetOptions = null)
        {
            if (testOptions == null)
            {
                testOptions = TestOptions;
            }

            if (testAspNetOptions == null)
            {
                testAspNetOptions = TestAspNetOptions;
            }

            TestContext = TestContextHelper.Instance();

            TestContext.Advanced.ResetMetricsValues();

            Server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddLogging();
                    services.AddRouting(options => { options.LowercaseUrls = true; });
                    services.AddMvc(options => { options.AddMetricsResourceFilter(); });
                    services.AddMetrics(options =>
                    {
                        options.DefaultSamplingType = testOptions.DefaultSamplingType;
                        options.DefaultGroupName = testOptions.DefaultGroupName;
                        options.DisableMetrics = testOptions.DisableMetrics;
                        options.DisableHealthChecks = testOptions.DisableHealthChecks;
                        options.JsonSchemeVersion = testOptions.JsonSchemeVersion;
                    }, TestContext)
                    .AddAspNetMetrics(options =>
                        {
                            options.HealthEnabled = testAspNetOptions.HealthEnabled;
                            options.MetricsEnabled = testAspNetOptions.MetricsEnabled;
                            options.MetricsTextEnabled = testAspNetOptions.MetricsTextEnabled;
                            options.PingEnabled = testAspNetOptions.PingEnabled;
                            options.HealthEndpoint = testAspNetOptions.HealthEndpoint;
                            options.MetricsEndpoint = testAspNetOptions.MetricsEndpoint;
                            options.MetricsTextEndpoint = testAspNetOptions.MetricsTextEndpoint;
                            options.PingEndpoint = testAspNetOptions.PingEndpoint;
                        });
                })
                .Configure(app =>
                {
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