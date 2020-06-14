using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

namespace EndpointTwo
{
    public static class PipelineSettingsExtensions
    {
        public static PipelineSettings StripAssemblyVersionFromEnclosedMessageTypePipeline(this PipelineSettings pipeline)
        {
            pipeline.Register(
                behavior: new StripAssemblyNameFromEnclosedMessageTypeOutgoingHeaderBehavior(),
                description: "Strips assembly version from enclosed message type");

            return pipeline;
        }
    }
    
    public class StripAssemblyNameFromEnclosedMessageTypeOutgoingHeaderBehavior : Behavior<IOutgoingPhysicalMessageContext>
    {
        public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
        {
            var headers = context.Headers;

            var currentType = headers["NServiceBus.EnclosedMessageTypes"];
            var newType = currentType.Substring(0, currentType.IndexOf(','));

            headers["NServiceBus.EnclosedMessageTypes"] = newType;

            return next();
        }
    }
}