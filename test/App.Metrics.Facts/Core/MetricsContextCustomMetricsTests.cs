using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Health;
using App.Metrics.MetricData;
using App.Metrics.Registries;
using App.Metrics.Sampling;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class MetricsContextCustomMetricsTests
    {
        private static readonly IOptions<AppMetricsOptions> Options
            = Microsoft.Extensions.Options.Options.Create(new AppMetricsOptions());

        private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();

        private static readonly IHealthCheckManager HealthCheckManager =
            new DefaultHealthCheckManager(LoggerFactory, new HealthCheckRegistry(LoggerFactory, Enumerable.Empty<HealthCheck>(), Options));
        private static readonly IMetricsDataManager MetricsDataManager =
            new DefaultMetricsDataManager(LoggerFactory, Options.Value.SystemClock, Enumerable.Empty<EnvironmentInfoEntry>());
        private static readonly IMetricsBuilder MetricsBuilder = new DefaultMetricsBuilder(Options.Value.SystemClock, Options.Value.DefaultSamplingType);
        private static readonly Func<IMetricsRegistry> MetricsRegistry = () => new DefaultMetricsRegistry();


        private readonly IMetricsContext _context = new DefaultMetricsContext(Options.Value.GlobalContextName,
            Options.Value.SystemClock, Options.Value.DefaultSamplingType,
            MetricsRegistry, MetricsBuilder, HealthCheckManager, MetricsDataManager);

        [Fact]
        public void can_register_custom_counter()
        {
            var counter = _context.Advanced.Counter("custom", Unit.Calls, () => new CustomCounter());
            counter.Should().BeOfType<CustomCounter>();
            counter.Increment();
            _context.Advanced.MetricsDataManager.GetMetricsData(_context).Counters.Single().Value.Count.Should().Be(10L);
        }

        [Fact]
        public void can_register_timer_with_custom_histogram()
        {
            var histogram = new CustomHistogram();

            var timer = _context.Advanced.Timer("custom", Unit.Calls, () => (IHistogramImplementation)histogram);

            timer.Record(10L, TimeUnit.Nanoseconds);

            histogram.Reservoir.Size.Should().Be(1);
            histogram.Reservoir.Values.Single().Should().Be(10L);
        }

        [Fact]
        public void can_register_timer_with_custom_reservoir()
        {
            var reservoir = new CustomReservoir();
            var timer = _context.Advanced.Timer("custom", Unit.Calls, () => (IReservoir)reservoir);

            timer.Record(10L, TimeUnit.Nanoseconds);

            reservoir.Size.Should().Be(1);
            reservoir.Values.Single().Should().Be(10L);
        }

        public class CustomCounter : ICounterImplementation
        {
            public CounterValue Value => new CounterValue(10L, new CounterValue.SetItem[0]);

            public void Decrement()
            {
            }

            public void Decrement(long value)
            {
            }

            public void Decrement(string item)
            {
            }

            public void Decrement(string item, long value)
            {
            }

            public CounterValue GetValue(bool resetMetric = false)
            {
                return Value;
            }

            public void Increment()
            {
            }

            public void Increment(long value)
            {
            }

            public void Increment(string item)
            {
            }

            public void Increment(string item, long value)
            {
            }

            public bool Merge(IMetricValueProvider<CounterValue> other)
            {
                return true;
            }

            public void Reset()
            {
            }
        }

        public class CustomHistogram : IHistogramImplementation
        {
            private bool _disposed = false;

            ~CustomHistogram()
            {
                Dispose(false);
            }

            public CustomReservoir Reservoir { get; } = new CustomReservoir();

            public HistogramValue Value
            {
                get { return new HistogramValue(Reservoir.Values.Last(), null, Reservoir.GetSnapshot()); }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            public void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        // Free any other managed objects here.
                    }
                }

                _disposed = true;
            }

            public HistogramValue GetValue(bool resetMetric = false)
            {
                return Value;
            }

            public void Reset()
            {
                Reservoir.Reset();
            }

            public void Update(long value, string userValue)
            {
                Reservoir.Update(value, userValue);
            }
        }

        public class CustomReservoir : IReservoir
        {
            private readonly List<long> _values = new List<long>();

            public long Count
            {
                get { return _values.Count; }
            }

            public int Size
            {
                get { return _values.Count; }
            }

            public IEnumerable<long> Values
            {
                get { return _values; }
            }

            public ISnapshot GetSnapshot(bool resetReservoir = false)
            {
                return new UniformSnapshot(_values.Count, _values);
            }

            public void Reset()
            {
                _values.Clear();
            }

            public void Update(long value, string userValue)
            {
                _values.Add(value);
            }
        }
    }
}