using System;
using System.Linq;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class MetricTagsTests : IDisposable
    {
        private readonly MetricsFixture _fixture;

        public MetricTagsTests()
        {
            //DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new MetricsFixture();
        }

        [Fact]
        public void can_get_tags_from_set_item_string()
        {
            var expectedTags = new MetricTags().With("item", "machine-1").With("item", "machine-2");

            var tags = new MetricTags();
            tags = tags.FromSetItemString("item:machine-1|item:machine-2");

            tags.Should().Equal(expectedTags);
        }

        [Fact]
        public void can_get_tags_from_set_item_string_when_single_item()
        {
            var expectedTags = new MetricTags().With("item", "item:machine-1");

            var tags = new MetricTags();
            tags = tags.FromSetItemString("item:machine-1");

            tags.Should().Equal(expectedTags);
        }

        [Fact]
        public void when_creating_tags_from_set_item_and_only_one_set_item_exists_without_a_colon_use_the_single_value_as_the_tag_value_with_key_item()
        {
            var expectedTags = new MetricTags().With("item", "machine-1").With("item", "machine-2");

            var tags = new MetricTags();
            tags = tags.FromSetItemString("machine-1|machine-2");

            tags.Should().Equal(expectedTags);
        }

        [Fact]
        public void can_get_tags_from_set_item_when_item_is_missing()
        {
            var expectedTags = MetricTags.None;

            var tags = new MetricTags();
            tags = tags.FromSetItemString(string.Empty);

            tags.Should().Equal(expectedTags);
        }

        [Fact]
        public void can_propergate_value_tags()
        {
            var tags = new MetricTags().With("tag", "value");
            var counterOptions = new CounterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = tags
            };

            var meterOptions = new MeterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = tags
            };

            var histogramOptions = new HistogramOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = tags
            };

            var timerOptions = new TimerOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = tags
            };

            _fixture.Metrics.Counter.Increment(counterOptions);
            _fixture.Metrics.Meter.Mark(meterOptions);
            _fixture.Metrics.Histogram.Update(histogramOptions, 1);
            _fixture.Metrics.Timer.Time(timerOptions, () => { });

            var data = _fixture.CurrentData(_fixture.Metrics);
            var context = data.Contexts.Single();

            context.Counters.Single().Tags.Should().Equal(tags);
            context.Meters.Single().Tags.Should().Equal(tags);
            context.Histograms.Single().Tags.Should().Equal(tags);
            context.Timers.Single().Tags.Should().Equal(tags);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fixture?.Dispose();
            }
        }
    }
}