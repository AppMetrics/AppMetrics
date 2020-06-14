using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

namespace EndpointTwo
{
    public class LoadSimulator
    {
        private readonly IMessageSession _messageSession;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly TimeSpan _minimumDelay;
        private readonly TimeSpan _idleDuration;
        private Task _fork;

        public LoadSimulator(IMessageSession messageSession, TimeSpan minimumDelay, TimeSpan idleDuration)
        {
            _messageSession = messageSession;
            _minimumDelay = minimumDelay;
            _idleDuration = TimeSpan.FromTicks(idleDuration.Ticks / 4);
        }

        public void Start()
        {
            _fork = Task.Run(Loop, CancellationToken.None);
        }

        private async Task Loop()
        {
            try
            {
                while (!_tokenSource.IsCancellationRequested)
                {
                    await Work().ConfigureAwait(false);
                    var delay = NextDelay();
                    await Task.Delay(delay, _tokenSource.Token).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private int _index;

        private TimeSpan NextDelay()
        {
            var angleInRadians = Math.PI / 180.0 * ++_index;
            var delay = TimeSpan.FromMilliseconds(_idleDuration.TotalMilliseconds * Math.Sin(angleInRadians));
            delay += _idleDuration;
            delay += _minimumDelay;
            return delay;
        }

        private Task Work()
        {
            return _messageSession.SendLocal(new SomeCommandTwo());
        }

        public Task Stop()
        {
            _tokenSource.Cancel();
            return _fork;
        }
    }
}