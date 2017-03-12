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

            result.Keys.Should().Equal("tag1", "tag2");
            result.Values.Should().Equal("value1", "value2");
        }

        [Fact]
        public void can_concat_tags()
        {
            var tags1 = new MetricTags("tag1", "value1");
            var tags2 = new MetricTags("tag2", "value2");

            var result = MetricTags.Concat(tags1, tags2);

            result.Keys.Should().Equal("tag1", "tag2");
            result.Values.Should().Equal("value1", "value2");
        }


        [Fact]
        public void can_get_tags_from_set_item_string()
        {
            var tags = new[] { "item", "item" };
            var values = new[] { "machine-1", "machine-2" };

            var expectedTags = new MetricTags(tags, values);

            var tag = MetricTags.FromSetItemString("item:machine-1,item:machine-2");

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
            var tag = MetricTags.FromSetItemString(string.Empty);

            Assert.Equal(tag, MetricTags.Empty);
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

        [Fact]
        public void count_should_be_one_when_single_key_value()
        {
            var tags = new MetricTags("key", "value");

            var count = tags.Count;

            count.Should().Be(1);
        }

        [Fact]
        public void count_should_total_key_values()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1", "machine-2" };

            var tags = new MetricTags(keys, values);

            var count = tags.Count;

            count.Should().Be(2);
        }

        public void Dispose() { Dispose(true); }

        [Fact]
        public void keys_and_values_be_same_length()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1" };

            Action setup = () =>
            {
                var unused = new MetricTags(keys, values);
            };

            setup.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void keys_cannot_be_null()
        {
            var values = new[] { "machine-1" };

            Action setup = () =>
            {
                var unused = new MetricTags(null, values);
            };

            setup.ShouldThrow<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void keys_cannot_contain_empty_strings_or_whitespace(string key)
        {
            var keys = new[] { key };
            var values = new[] { "machine-1" };

            Action setup = () =>
            {
                var unused = new MetricTags(keys, values);
            };

            setup.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void keys_cannot_contain_nulls()
        {
            var keys = new[] { null, "key2" };
            var values = new[] { "machine-1", "machine-2" };

            Action setup = () =>
            {
                var unused = new MetricTags(keys, values);
            };

            setup.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void tags_with_different_keys_should_not_be_equal_with_operator()
        {
            var keysLeft = new[] { "key1", "key2" };
            var keysRight = new[] { "key1", "key3" };
            var values = new[] { "machine-1", "machine-2" };

            var left = new MetricTags(keysLeft, values);
            var right = new MetricTags(keysRight, values);

            Assert.True(left != right);
        }

        [Fact]
        public void tags_with_different_values_should_not_be_equal()
        {
            var keys = new[] { "key1", "key2" };
            var valuesLeft = new[] { "machine-1", "machine-2" };
            var valuesRight = new[] { "machine-1", "machine-3" };

            var left = new MetricTags(keys, valuesLeft);
            var right = new MetricTags(keys, valuesRight);

            left.Equals(right).Should().Be(false);
        }

        [Fact]
        public void tags_with_same_key_values_should_be_equal()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1", "machine-2" };

            var left = new MetricTags(keys, values);
            var right = new MetricTags(keys, values);

            left.Equals(right).Should().Be(true);
        }

        [Fact]
        public void tags_with_same_key_values_should_be_equal_with_operator()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1", "machine-2" };

            var left = new MetricTags(keys, values);
            var right = new MetricTags(keys, values);

            Assert.True(left == right);
        }

        [Fact]
        public void when_creating_tags_from_set_item_and_only_one_set_item_exists_without_a_colon_use_the_single_value_as_the_tag_value_with_key_item(

        )
        {
            var tags = MetricTags.FromSetItemString("machine-1,machine-2");

            tags.Keys.Should().Equal("item", "item");
            tags.Values.Should().Equal("machine-1", "machine-2");
        }

        [Fact]
        public void can_convert_to_multidimensional_metric_name()
        {
            var keys = new[] { "key1", "key2", "key3", "key4", "key5", "key6", "key7", "key8" };
            var values = new[] { "value1", "value2", "value3", "value4", "value5", "value6", "value7", "value8" };

            var tags = new MetricTags(keys, values);

            tags.AsMetricName("metric_name").Should().Be("metric_name|key1:value1,key2:value2,key3:value3,key4:value4,key5:value5,key6:value6,key7:value7,key8:value8");
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