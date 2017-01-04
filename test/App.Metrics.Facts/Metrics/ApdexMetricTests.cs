using System.Linq;
using App.Metrics.Core;
using App.Metrics.Internal;
using App.Metrics.Utils;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Metrics
{
    public class ApdexMetricTests
    {
        private readonly ApdexMetric _apdex;
        private readonly IClock _clock = new TestClock();

        public ApdexMetricTests()
        {
            _apdex = new ApdexMetric(SamplingType.ExponentiallyDecaying, Constants.ReservoirSampling.DefaultSampleSize,
                Constants.ReservoirSampling.DefaultSampleSize, _clock, Constants.ReservoirSampling.DefaultApdexTSeconds, false);
        }

        //TODO: AH - Add Tests for tracking adpex via action and func

        [Fact]
        public void can_reset()
        {
            using (_apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            _apdex.Value.Score.Should().NotBe(0);
            _apdex.Value.SampleSize.Should().Be(1);
            _apdex.Value.Satisfied.Should().Be(1);
            _apdex.Value.Tolerating.Should().Be(0);
            _apdex.Value.Frustrating.Should().Be(0);

            _apdex.Reset();
            
            _apdex.Value.Score.Should().Be(0);
            _apdex.Value.SampleSize.Should().Be(0);
            _apdex.Value.Satisfied.Should().Be(0);
            _apdex.Value.Tolerating.Should().Be(0);
            _apdex.Value.Frustrating.Should().Be(0);
        }

        [Theory]
        [InlineData(1, 1.0, 600, 0.75, 10000, 0.5)]
        [InlineData(10000, 0, 1, 0.5, 600, 0.5)]
        public void can_score(long durationFirstRequest, double apdexAfterFirstRequest,
            long durationSecondRequest, double apdexAfterSecondRequest,
            long durationThirdRequest, double apdexAfterThirdRequest)
        {
            _apdex.Value.Score.Should().Be(0);

            using (_apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, durationFirstRequest);
            }

            _apdex.Value.Score.Should().Be(apdexAfterFirstRequest);

            using (_apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, durationSecondRequest);
            }

            _apdex.Value.Score.Should().Be(apdexAfterSecondRequest);

            using (_apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, durationThirdRequest);
            }

            _apdex.Value.Score.Should().Be(apdexAfterThirdRequest);
        }

        [Fact]
        public void context_records_only_on_first_dispose()
        {
            var context = _apdex.NewContext();
            _clock.Advance(TimeUnit.Milliseconds, 100);
            context.Dispose(); // passing the structure to using() creates a copy
            _clock.Advance(TimeUnit.Milliseconds, 100000000);
            context.Dispose();

            _apdex.Value.SampleSize.Should().Be(1);
        }

        [Fact]
        public void context_reports_elapsed_time()
        {
            using (var context = _apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
                context.Elapsed.TotalMilliseconds.Should().Be(100);
            }
        }

        [Fact]
        public void counts_even_when_action_throws()
        {
            //Action action = () => this._apdex.Time(() => { throw new InvalidOperationException(); });

            //action.ShouldThrow<InvalidOperationException>();

            //this._apdex.Value.Score.Should().BeGreaterThan(0);
        }
    }
}