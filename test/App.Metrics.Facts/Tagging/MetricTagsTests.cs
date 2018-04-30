// <copyright file="MetricTagsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Counter;
using App.Metrics.FactsCommon.Fixtures;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Tagging
{
    public class MetricTagsTests : IDisposable
    {
        private readonly MetricsFixture _fixture;

        public MetricTagsTests()
        {
            // DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new MetricsFixture();
        }

        [Fact]
        public void Can_concat_tags()
        {
            var tags1 = new MetricTags("tag1", "value1");
            var tags2 = new MetricTags("tag2", "value2");

            var result = MetricTags.Concat(tags1, tags2);

            result.Keys.Should().Equal("tag1", "tag2");
            result.Values.Should().Equal("value1", "value2");
        }

        [Fact]
        public void Can_concat_with_dictionary()
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
        public void Can_convert_to_dictionary()
        {
            var tags = new MetricTags(new[] { "key_1", "key_2" }, new[] { "value 1", "value 2" });

            var dictionary = tags.ToDictionary();

            dictionary.Count.Should().Be(2);
            dictionary.Keys.Should().Contain("key_1");
            dictionary.Keys.Should().Contain("key_2");
            dictionary.Values.Should().Contain("value 1");
            dictionary.Values.Should().Contain("value 2");
        }

        [Fact]
        public void Can_convert_to_dictionary_and_format_values()
        {
            var tags = new MetricTags(new[] { "key_1", "key_2" }, new[] { "value 1", "value 2" });

            var dictionary = tags.ToDictionary(tagValue => tagValue.Replace(' ', '_'));

            dictionary.Count.Should().Be(2);
            dictionary.Keys.Should().Contain("key_1");
            dictionary.Keys.Should().Contain("key_2");
            dictionary.Values.Should().Contain("value_1");
            dictionary.Values.Should().Contain("value_2");
        }

        [Fact]
        public void Can_convert_to_multidimensional_metric_name()
        {
            var keys = new[] { "key1", "key2", "key3", "key4", "key5", "key6", "key7", "key8" };
            var values = new[] { "value1", "value2", "value3", "value4", "value5", "value6", "value7", "value8" };

            var tags = new MetricTags(keys, values);

            tags.AsMetricName("metric_name").Should().Be(
                "metric_name|key1:value1,key2:value2,key3:value3,key4:value4,key5:value5,key6:value6,key7:value7,key8:value8");
        }

        [Fact]
        public void Can_get_tags_from_set_item_string()
        {
            var tags = new[] { "item", "item" };
            var values = new[] { "machine-1", "machine-2" };

            var expectedTags = new MetricTags(tags, values);

            var tag = MetricTags.FromSetItemString("item:machine-1,item:machine-2");

            Assert.Equal(expectedTags, tag);
        }

        [Fact]
        public void Can_get_tags_from_set_item_string_when_single_item()
        {
            var expectedTags = new MetricTags("item", "item:machine-1");

            var tag = MetricTags.FromSetItemString("item:machine-1");

            Assert.Equal(expectedTags, tag);
        }

        [Fact]
        public void Can_get_tags_from_set_item_when_item_is_missing()
        {
            var tag = MetricTags.FromSetItemString(string.Empty);

            Assert.Equal(tag, MetricTags.Empty);
        }

        [Fact]
        public void Can_propergate_value_tags()
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

            context.Counters.Single().Tags.Should().BeEquivalentTo(tags);
            context.Meters.Single().Tags.Should().BeEquivalentTo(tags);
            context.Histograms.Single().Tags.Should().BeEquivalentTo(tags);
            context.Timers.Single().Tags.Should().BeEquivalentTo(tags);
        }

        [Fact]
        public void Count_should_be_one_when_single_key_value()
        {
            var tags = new MetricTags("key", "value");

            var count = tags.Count;

            count.Should().Be(1);
        }

        [Fact]
        public void Count_should_total_key_values()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1", "machine-2" };

            var tags = new MetricTags(keys, values);

            var count = tags.Count;

            count.Should().Be(2);
        }

        public void Dispose() { Dispose(true); }

        [Fact]
        public void Keys_and_values_be_same_length()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1" };

            Action setup = () =>
            {
                var unused = new MetricTags(keys, values);
            };

            setup.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Keys_cannot_be_null()
        {
            var values = new[] { "machine-1" };

            Action setup = () =>
            {
                var unused = new MetricTags(null, values);
            };

            setup.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Keys_cannot_contain_empty_strings_or_whitespace(string key)
        {
            var keys = new[] { key };
            var values = new[] { "machine-1" };

            Action setup = () =>
            {
                var unused = new MetricTags(keys, values);
            };

            setup.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Keys_cannot_contain_nulls()
        {
            var keys = new[] { null, "key2" };
            var values = new[] { "machine-1", "machine-2" };

            Action setup = () =>
            {
                var unused = new MetricTags(keys, values);
            };

            setup.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Tags_with_different_keys_should_not_be_equal_with_operator()
        {
            var keysLeft = new[] { "key1", "key2" };
            var keysRight = new[] { "key1", "key3" };
            var values = new[] { "machine-1", "machine-2" };

            var left = new MetricTags(keysLeft, values);
            var right = new MetricTags(keysRight, values);

            Assert.True(left != right);
        }

        [Fact]
        public void Tags_with_different_values_should_not_be_equal()
        {
            var keys = new[] { "key1", "key2" };
            var valuesLeft = new[] { "machine-1", "machine-2" };
            var valuesRight = new[] { "machine-1", "machine-3" };

            var left = new MetricTags(keys, valuesLeft);
            var right = new MetricTags(keys, valuesRight);

            left.Equals(right).Should().Be(false);
        }

        [Fact]
        public void Tags_with_same_key_values_should_be_equal()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1", "machine-2" };

            var left = new MetricTags(keys, values);
            var right = new MetricTags(keys, values);

            left.Equals(right).Should().Be(true);
        }

        [Fact]
        public void Tags_with_same_key_values_should_be_equal_with_operator()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "machine-1", "machine-2" };

            var left = new MetricTags(keys, values);
            var right = new MetricTags(keys, values);

            Assert.True(left == right);
        }

        [Fact]
        public void
            When_creating_tags_from_set_item_and_only_one_set_item_exists_without_a_colon_use_the_single_value_as_the_tag_value_with_key_item()
        {
            var tags = MetricTags.FromSetItemString("machine-1,machine-2");

            tags.Keys.Should().Equal("item", "item");
            tags.Values.Should().Equal("machine-1", "machine-2");
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