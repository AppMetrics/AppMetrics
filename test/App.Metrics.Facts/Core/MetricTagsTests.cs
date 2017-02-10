// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Tagging;
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
        public void can_concat_with_dictionary()
        {
            var tags1 = new MetricTags("tag1", "value1");
            var tags2 = new Dictionary<string, string>
                        {
                            { "tag2", "value2" }
                        };

            var result = MetricTags.Concat(tags1, tags2);

            result.Keys.Should().Equal(new[] { "tag1", "tag2" });
            result.Values.Should().Equal(new[] { "value1", "value2" });
        }

        [Fact]
        public void can_get_tags_from_set_item_string()
        {
            var tags = new[] { "item", "item" };
            var values = new[] { "machine-1", "machine-2" };

            var expectedTags = new MetricTags(tags, values);

            var tag = MetricTags.FromSetItemString("item:machine-1|item:machine-2");

            Assert.Equal(expectedTags, tag);
        }

        [Fact]
        public void can_get_tags_from_set_item_string_when_single_item()
        {
            var expectedTags = new MetricTags("item", "item:machine-1");

            var tag = MetricTags.FromSetItemString("item:machine-1");

            Assert.Equal(expectedTags, tag);
        }

        [Fact]
        public void can_get_tags_from_set_item_when_item_is_missing()
        {
            var expectedTags = MetricTags.Empty;

            var tag = MetricTags.FromSetItemString(string.Empty);

            Assert.Equal(expectedTags.ToString(), tag.ToString());
        }

        [Fact]
        public void can_propergate_value_tags()
        {
            var tags = new MetricTags("tag", "value");
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

            _fixture.Metrics.Measure.Counter.Increment(counterOptions);
            _fixture.Metrics.Measure.Meter.Mark(meterOptions);
            _fixture.Metrics.Measure.Histogram.Update(histogramOptions, 1);
            _fixture.Metrics.Measure.Timer.Time(timerOptions, () => { });

            var data = _fixture.CurrentData(_fixture.Metrics);
            var context = data.Contexts.Single();

            context.Counters.Single().Tags.ShouldBeEquivalentTo(tags);
            context.Meters.Single().Tags.ShouldBeEquivalentTo(tags);
            context.Histograms.Single().Tags.ShouldBeEquivalentTo(tags);
            context.Timers.Single().Tags.ShouldBeEquivalentTo(tags);
        }

        public void Dispose() { Dispose(true); }

        [Fact]
        public void when_creating_tags_from_set_item_and_only_one_set_item_exists_without_a_colon_use_the_single_value_as_the_tag_value_with_key_item(
        )
        {
            var tags = MetricTags.FromSetItemString("machine-1|machine-2");

            tags.Keys.Should().Equal(new[] { "item", "item" });
            tags.Values.Should().Equal(new[] { "machine-1", "machine-2" });
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