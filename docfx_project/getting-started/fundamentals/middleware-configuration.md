# AspNet Middleware Configuration Options

App Metrics includes an [AspNet Middleware Extension](../intro.md) which will record things like error rates per endpoint and an overall rate, request rates per endpoint and an overall rate etc.

Configuration options are accessible through the `AddMetricsMiddleware` extension on the `IMetricsHostBuilder`. 

These configuration options include:

- **HealthEndpoint**: Allows customization of the endpoint which will report the health status of the application. The default path is `/health`
- **HealthEndpointEnabled**: Allows enabling/disabling of the `/health` endpoint, when disabled will result in a 404 status code, the default is true.
- **IgnoredRoutesRegexPatterns**: The AspNet Middleware Extension measures for example a Timer metric for each request, specific endpoints can be ignored by providing an `IList<string>` of regex patterns. 
- **MetricsEndpoint**: Allows customization of the endpoint which will report a snapshot of the applications current metric measurements. The default path is `/metrics`
- **MetricsEndpointEnabled**: Allows enabling/disabling of the `/metrics` endpoint, when disabled will result in a 404 status code, the default is true.
- **MetricsTextEndpoint**: Allows customization of the endpoint which will report a snapshot of the applications current metric measurements in a hummanized format. The default path is `/metrics-text`
- **MetricsTextEndpointEnabled**: Allows enabling/disabling of the `/metrics-text` endpoint, when disabled will result in a 404 status code, the default is true.
- **OAuth2TrackingEnabled**: Allows recording of all OAuth2 Client tracking to be disabled. When enabled middleware is configured to track request rates, an overall percentage of requests per Client / per endpoint, and error rates per Client.
- **PingEndpoint**: Allows customization of the ping endpoint which simply returns pong. The default path is `/ping`
- **PingEndpointEnabled**: Allows enabling/disabling of the `/ping` endpoint, when disabled will result in a 404 status code, the default is true.

----------

### Changing options using `Action<AspNetMetricsOptions>`

[!code-csharp[Main](../../src/samples/StartupWithAspNetOptions.cs?highlight=8)]      	     

----------

### Changing options using `Microsoft.Extensions.Configuration.IConfiguration`

Below is an example `Startup.cs` using `appsettings.json` as a configuration source:

[!code-csharp[Main](../../src/samples/StartupWithAspNetIConfiguration.cs?highlight=20)]      	     

And the `appsettings.json` file

[!code-json[Main](../../src/samples/AspNetOptions.js)]

----------

### Next Steps

- [App MetricsConfiguration Options](configuration.md)
- [Sample Applications](../../samples/index.md)
