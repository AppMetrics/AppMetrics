using App.Metrics.Sampling;
using App.Metrics.Utils;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks
{
    public class ExponentiallyDecayingResevoirBenchmarks
    {
        private readonly Clock.TestClock _clock = new Clock.TestClock();
        private readonly TestScheduler _scheduler;

        public ExponentiallyDecayingResevoirBenchmarks()
        {
            _scheduler = new TestScheduler(_clock);
        }

        [Benchmark]
        public void EDR_HeavilyBiasedReservoirOf100OutOf1000Elements()
        {
            var reservoir = new ExponentiallyDecayingReservoir(1000, 0.01);
            for (var i = 0; i < 100; i++)
            {
                reservoir.Update(i);
            }

            var snapshot = reservoir.GetSnapshot();
        }

        [Benchmark]
        public void EDR_longPeriodsOfInactivityShouldNotCorruptSamplingState()
        {
            var reservoir = new ExponentiallyDecayingReservoir(10, 0.015, _clock, _scheduler);

            // add 1000 values at a rate of 10 values/second
            for (var i = 0; i < 1000; i++)
            {
                reservoir.Update(1000 + i);
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            reservoir.GetSnapshot();
            reservoir.GetSnapshot();

            // wait for 15 hours and add another value.
            // this should trigger a rescale. Note that the number of samples will be reduced to 2
            // because of the very small scaling factor that will make all existing priorities equal to
            // zero after rescale.
            _clock.Advance(TimeUnit.Hours, 15);
            reservoir.Update(2000);
            var snapshot = reservoir.GetSnapshot();

            // add 1000 values at a rate of 10 values/second
            for (var i = 0; i < 1000; i++)
            {
                reservoir.Update(3000 + i);
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            var finalSnapshot = reservoir.GetSnapshot();
        }

        [Benchmark]
        public void EDR_QuantiliesShouldBeBasedOnWeights()
        {
            var reservoir = new ExponentiallyDecayingReservoir(_clock, _scheduler);

            for (var i = 0; i < 40; i++)
            {
                reservoir.Update(177);
            }

            _clock.Advance(TimeUnit.Seconds, 120);

            for (var i = 0; i < 10; i++)
            {
                reservoir.Update(9999);
            }

            reservoir.GetSnapshot();
        }

        [Benchmark]
        public void EDR_RecordsUserValue()
        {
            var reservoir = new ExponentiallyDecayingReservoir(_clock, _scheduler);

            reservoir.Update(2L, "B");
            reservoir.Update(1L, "A");

            reservoir.GetSnapshot();
        }

        [Benchmark]
        public void EDR_ReservoirOf100OutOf1000Elements()
        {
            var reservoir = new ExponentiallyDecayingReservoir(100, 0.99);
            for (var i = 0; i < 1000; i++)
            {
                reservoir.Update(i);
            }

            var snapshot = reservoir.GetSnapshot();
        }

        [Benchmark]
        public void EDR_ReservoirOf100OutOf10Elements()
        {
            var reservoir = new ExponentiallyDecayingReservoir(100, 0.99);
            for (var i = 0; i < 10; i++)
            {
                reservoir.Update(i);
            }

            var snapshot = reservoir.GetSnapshot();
        }
    }
}