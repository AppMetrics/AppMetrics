using System.Net.Http;
using Metrics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using AspNet.Metrics;
using Builder;
using Builder;

namespace AspNet.Metrics.Facts.Middleware
{

    public class PostAndPutRequestSizeHistogramMiddlewareFacts
    {
        private const int timePerRequest = 100;
        private readonly HttpClient _client;
        private readonly MetricsConfig _config;

        private readonly TestContext _context = new TestContext();
        private readonly TestServer _server;

        public PostAndPutRequestSizeHistogramMiddlewareFacts()
        {
            _config = new MetricsConfig(_context);

            _server = new TestServer(new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseMetrics();
                }));
            _client = _server.CreateClient();
        }
    }
}