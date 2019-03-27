// <copyright file="MetricsFilterBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class MetricsFilterBuilderTests
    {
        [Fact]
        public void Cannot_set_null_filter()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var unused = new MetricsBuilder().Filter.With(filter: null);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}