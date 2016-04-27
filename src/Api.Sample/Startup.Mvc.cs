using AspNet.Metrics.Infrastructure;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Api.Sample
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddDefaultJsonOptions(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters = new JsonConverter[]
                {
                    new StringEnumConverter { AllowIntegerValues = true, CamelCaseText = true }
                };
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            return mvcBuilder;
        }

        public static IApplicationBuilder UseMvcWithMetrics(this IApplicationBuilder app)
        {
            app.UseMetrics();
            app.UseMvc(routes => routes.DefaultHandler = new MetricsRouteHandler(new MvcRouteHandler()));

            return app;
        }
    }
}