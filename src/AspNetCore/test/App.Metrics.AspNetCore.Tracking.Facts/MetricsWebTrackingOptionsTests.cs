// <copyright file="MetricsWebTrackingOptionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace App.Metrics.AspNetCore.Tracking.Facts
{
    public class MetricsWebTrackingOptionsTests
    {
        [Fact]
        public void Patterns_and_regex_patterns_have_default_state()
        {
            var options = new MetricsWebTrackingOptions();

            options.IgnoredRoutesRegexPatterns.Should().NotBeNull();
            options.IgnoredRoutesRegex.Should().NotBeNull();
        }

        [Fact]
        public void Setting_ignored_routes_sets_regex_patters()
        {
            var patterns = new List<string> { "abc", "123" };

            var options = new MetricsWebTrackingOptions();
            options.IgnoredRoutesRegexPatterns = patterns;

            options.IgnoredRoutesRegexPatterns
                   .Should().BeEquivalentTo(patterns);
            options.IgnoredRoutesRegex.Select(r => r.ToString())
                   .Should().BeEquivalentTo(patterns);
        }

        [Fact]
        public void Changing_ignored_routes_changes_regex_patters()
        {
            var options = new MetricsWebTrackingOptions();
            options.IgnoredRoutesRegexPatterns.Add("abc");
            options.IgnoredRoutesRegexPatterns.Add("123");
            options.IgnoredRoutesRegexPatterns.Add("route1");
            options.IgnoredRoutesRegexPatterns.Add("route2");

            options.IgnoredRoutesRegexPatterns.RemoveAt(1);
            options.IgnoredRoutesRegexPatterns.Remove("route2");
            options.IgnoredRoutesRegexPatterns.Insert(1, "qwe");
            options.IgnoredRoutesRegexPatterns[0] = "xyz";

            var expected = new List<string> { "xyz", "qwe", "route1" };
            options.IgnoredRoutesRegexPatterns
                   .Should().BeEquivalentTo(expected);
            options.IgnoredRoutesRegex.Select(r => r.ToString())
                   .Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Can_clear_ignored_routes_and_regex()
        {
            var options = new MetricsWebTrackingOptions();
            options.IgnoredRoutesRegexPatterns = new List<string> { "abc", "123" };

            options.IgnoredRoutesRegexPatterns.Clear();

            options.IgnoredRoutesRegexPatterns.Should().BeEmpty();
            options.IgnoredRoutesRegex.Should().BeEmpty();
        }
    }
}
