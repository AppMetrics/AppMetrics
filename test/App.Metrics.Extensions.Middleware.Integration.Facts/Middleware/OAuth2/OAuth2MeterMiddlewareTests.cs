using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using App.Metrics.Extensions.Middleware.Internal;
using App.Metrics.Meter;
using App.Metrics.Meter.Extensions;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.OAuth2
{
    public class OAuth2MeterMiddlewareTests : IClassFixture<MetricsHostTestFixture<OAuthTestStartup>>
    {
        public OAuth2MeterMiddlewareTests(MetricsHostTestFixture<OAuthTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task can_track_status_codes_per_client_when_oauth2_tracking_enabled()
        {
            await Client.GetAsync("/api/test/oauth/client1");
            await Client.GetAsync("/api/test/oauth/client2");
            await Client.GetAsync("/api/test/oauth/error/client1");
            await Client.GetAsync("/api/test/oauth/error/client2");
            await Client.GetAsync("/api/test/oauth/error/client3");
            await Client.GetAsync("/api/test/oauth/error/client4");

            Func<string, MeterValue> getMeterValue = metricName => Context.Snapshot.GetMeterValue(
                OAuth2MetricsRegistry.ContextName,
                metricName);

            var successItems = getMeterValue("GET api/test/oauth/{clientid} Http Requests").Items;
            var errorItems = getMeterValue("GET api/test/oauth/error/{clientid} Http Requests").Items;
            var overallItems = getMeterValue("Http Requests").Items;

            successItems.Should().HaveCount(2);
            errorItems.Should().HaveCount(4);
            overallItems.Should().HaveCount(6);

            AssertSuccessItems(successItems);
            AssertErrorItems(errorItems);
            AssertOverallItems(overallItems);
        }

        private static void AssertSuccessItems(MeterValue.SetItem[] successItems)
        {
            var clientOneItem = successItems.FirstOrDefault(i => i.Item == "client_id:client1|http_status_code:200");
            var clientTwoItem = successItems.FirstOrDefault(i => i.Item == "client_id:client2|http_status_code:200");

            clientOneItem.Should().NotBeNull();
            clientOneItem.Percent.Should().Be(50);

            clientTwoItem.Should().NotBeNull();
            clientTwoItem.Percent.Should().Be(50);
        }

        private static void AssertOverallItems(MeterValue.SetItem[] overallItems)
        {
            overallItems.FirstOrDefault(i => i.Item == "client_id:client1|http_status_code:200").Should().NotBeNull();
            overallItems.FirstOrDefault(i => i.Item == "client_id:client2|http_status_code:200").Should().NotBeNull();
            overallItems.FirstOrDefault(i => i.Item == "client_id:client1|http_status_code:500").Should().NotBeNull();
            overallItems.FirstOrDefault(i => i.Item == "client_id:client2|http_status_code:500").Should().NotBeNull();
            overallItems.FirstOrDefault(i => i.Item == "client_id:client3|http_status_code:500").Should().NotBeNull();
            overallItems.FirstOrDefault(i => i.Item == "client_id:client4|http_status_code:500").Should().NotBeNull();
        }

        private static void AssertErrorItems(MeterValue.SetItem[] errorItems)
        {
            var clientOneItem = errorItems.FirstOrDefault(i => i.Item == "client_id:client1|http_status_code:500");
            var clientTwoItem = errorItems.FirstOrDefault(i => i.Item == "client_id:client2|http_status_code:500");
            var clientThreeItem = errorItems.FirstOrDefault(i => i.Item == "client_id:client3|http_status_code:500");
            var clientFourItem = errorItems.FirstOrDefault(i => i.Item == "client_id:client4|http_status_code:500");

            clientOneItem.Should().NotBeNull();
            clientOneItem.Percent.Should().Be(25);

            clientTwoItem.Should().NotBeNull();
            clientTwoItem.Percent.Should().Be(25);

            clientThreeItem.Should().NotBeNull();
            clientThreeItem.Percent.Should().Be(25);

            clientFourItem.Should().NotBeNull();
            clientFourItem.Percent.Should().Be(25);
        }
    }
}