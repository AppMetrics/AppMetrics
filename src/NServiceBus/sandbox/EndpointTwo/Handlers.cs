using System;
using System.Threading.Tasks;
using NServiceBus;

namespace EndpointTwo
{
    public class SomeCommandTwoHandler : IHandleMessages<SomeCommandTwo>
    {
        private static readonly Random Random = new Random();

        public async Task Handle(SomeCommandTwo message, IMessageHandlerContext context)
        {
            await Task.Delay(Random.Next(50, 250)).ConfigureAwait(false);

            if (Random.Next(10) <= 1) throw new Exception("Random 10% chaos!");

            await context.Publish(new SomeEventTwo());
        }
    }
    
    public class SomeEventTwoHandler : IHandleMessages<SomeEventTwo>
    {
        private static readonly Random Random = new Random();

        public async Task Handle(SomeEventTwo message, IMessageHandlerContext context)
        {
            await Task.Delay(Random.Next(50, 250)).ConfigureAwait(false);

            if (Random.Next(10) <= 1) throw new Exception("Random 10% chaos!");
        }
    }
}