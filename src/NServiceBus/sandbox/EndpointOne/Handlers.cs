using System;
using System.Threading.Tasks;
using NServiceBus;

namespace EndpointOne
{
    public class SomeCommandOneHandler : IHandleMessages<SomeCommandOne>
    {
        private static readonly Random Random = new Random();

        public async Task Handle(SomeCommandOne message, IMessageHandlerContext context)
        {
            await Task.Delay(Random.Next(50, 250)).ConfigureAwait(false);

            if (Random.Next(10) <= 1) throw new Exception("Random 10% chaos!");

            await context.Publish(new SomeEventOne());
        }
    }
    
    public class SomeEventOneHandler : IHandleMessages<SomeEventOne>
    {
        private static readonly Random Random = new Random();

        public async Task Handle(SomeEventOne message, IMessageHandlerContext context)
        {
            await Task.Delay(Random.Next(50, 250)).ConfigureAwait(false);

            if (Random.Next(10) <= 1) throw new Exception("Random 10% chaos!");
        }
    }
}