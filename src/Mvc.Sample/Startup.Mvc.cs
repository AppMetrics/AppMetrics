using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AspNet.Metrics.Infrastructure;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Abstractions;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Mvc.Internal;
using Microsoft.AspNet.Mvc.Routing;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mvc.Sample
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

            var router = new MetricsRouteHandler(new MvcRouteHandler());

            var routes = new RouteBuilder
            {
                DefaultHandler = router,
                ServiceProvider = app.ApplicationServices
            };

            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");

            routes.Routes.Insert(0, AttributeRouting.CreateAttributeMegaRoute(
               routes.DefaultHandler,
               app.ApplicationServices));

            return app.UseRouter(routes.Build());
        }
    }
}