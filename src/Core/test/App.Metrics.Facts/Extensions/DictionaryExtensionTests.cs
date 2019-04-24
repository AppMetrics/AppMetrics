// <copyright file="DictionaryExtensionTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Extensions
{
    public class DictionaryExtensionTests
    {
        [Fact]
        public void Does_add_double_if_not_nan_or_infinity()
        {
            // Arrange
            var sut = new Dictionary<string, object>();

            // Act
            sut.AddIfNotNanOrInfinity("key", 1.0);

            // Assert
            sut.Count.Should().Be(1);
        }

        [Fact]
        public void Does_add_string_if_present()
        {
            // Arrange
            var sut = new Dictionary<string, object>();

            // Act
            sut.AddIfPresent("key", "string");

            // Assert
            sut.Count.Should().Be(1);
        }

        [Theory]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NegativeInfinity)]
        public void Does_not_add_double_if_infinity(double value)
        {
            // Arrange
            var sut = new Dictionary<string, object>();

            // Act
            sut.AddIfNotNanOrInfinity("key", value);

            // Assert
            sut.Count.Should().Be(0);
        }

        [Fact]
        public void Does_not_add_double_if_nan()
        {
            // Arrange
            var sut = new Dictionary<string, object>();

            // Act
            sut.AddIfNotNanOrInfinity("key", double.NaN);

            // Assert
            sut.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Does_not_add_string_if_missing(string value)
        {
            // Arrange
            var sut = new Dictionary<string, object>();

            // Act
            sut.AddIfPresent("key", value);

            // Assert
            sut.Count.Should().Be(0);
        }
    }
}