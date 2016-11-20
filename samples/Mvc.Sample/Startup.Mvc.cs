using Microsoft.Extensions.DependencyInjection;
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
    }
}