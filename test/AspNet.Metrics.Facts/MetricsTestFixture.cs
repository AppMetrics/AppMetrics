using System;
using System.Net.Http;
using App.Metrics;
using App.Metrics.DependencyInjection;
using App.Metrics.Json;
using App.Metrics.Utils;
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
            Clock = new Clock.TestClock()
        };

        private static readonly AspNetMetricsOptions TestAspNetOptions = new AspNetMetricsOptions
        {
            MetricsTextEndpointEnabled = true,
            HealthEndpointEnabled = true,
            MetricsEndpointEnabled = true,
            PingEndpointEnabled = true
        };

        public MetricsTestFixture(AppMetricsOptions testOptions = null,
            AspNetMetricsOptions testAspNetOptions = null, bool enableHealthChecks = true)
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
                    }, TestContext)
                    .AddHealthChecks(options =>
                        {
                            options.IsEnabled = enableHealthChecks;
                        })
                    .AddAspNetMetrics(options =>
                        {
                            options.HealthEndpointEnabled = testAspNetOptions.HealthEndpointEnabled;
                            options.MetricsEndpointEnabled = testAspNetOptions.MetricsEndpointEnabled;
                            options.MetricsTextEndpointEnabled = testAspNetOptions.MetricsTextEndpointEnabled;
                            options.PingEndpointEnabled = testAspNetOptions.PingEndpointEnabled;
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