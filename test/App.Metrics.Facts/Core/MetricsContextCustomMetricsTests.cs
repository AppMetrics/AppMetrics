using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.MetricData;
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
            new DefaultHealthCheckManager(LoggerFactory, () => new ConcurrentDictionary<string, HealthCheck>());

        private static readonly Func<string, IMetricGroupRegistry> NewMetricsGroupRegistry = name => new DefaultMetricGroupRegistry(name);


        private static readonly IMetricsRegistry Registry = new DefaultMetricsRegistry(LoggerFactory, Options, 
            new EnvironmentInfoBuilder(LoggerFactory), NewMetricsGroupRegistry);

        private static readonly IMetricsDataManager MetricsDataManager =  new DefaultMetricsDataManager(Registry);

        private static readonly IMetricsBuilder MetricsBuilder = new DefaultMetricsBuilder(Options.Value.Clock);
       
        private readonly IMetricsContext _context = new DefaultMetricsContext(Options, Registry, MetricsBuilder, HealthCheckManager, MetricsDataManager);

        public MetricsContextCustomMetricsTests()
        {
            _context.Advanced.ResetMetricsValues();
        }

        [Fact]
        public async Task can_register_custom_counter()
        {
            var counterOptions = new CounterOptions
            {
                Name = "Custom Counter",
                MeasurementUnit = Unit.Calls
            };
            var counter = _context.Advanced.Counter(counterOptions, () => new CustomCounter());
            counter.Should().BeOfType<CustomCounter>();
            counter.Increment();

            var data = await _context.Advanced.DataManager.GetMetricsDataAsync();
            var group = data.Groups.Single();

            group.Counters.Single().Value.Count.Should().Be(10L);
        }

        [Fact]
        public void can_register_timer_with_custom_histogram()
        {
            var histogram = new CustomHistogram();
            var timerOptions = new TimerOptions
            {
                Name = "custom",
                MeasurementUnit = Unit.Calls
            };

            var timer = _context.Advanced.Timer(timerOptions, () => (IHistogramImplementation)histogram);

            timer.Record(10L, TimeUnit.Nanoseconds);

            histogram.Reservoir.Size.Should().Be(1);
            histogram.Reservoir.Values.Single().Should().Be(10L);
        }

        [Fact]
        public void can_register_timer_with_custom_reservoir()
        {
            var reservoir = new CustomReservoir();
            var timerOptions = new TimerOptions
            {
                Name = "custom",
                MeasurementUnit = Unit.Calls
            };
            var timer = _context.Advanced.Timer(timerOptions, () => (IReservoir)reservoir);

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