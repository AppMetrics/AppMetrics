// <copyright file="MetricsFilterBuilderTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
                var builder = new MetricsBuilder().Filter.With(filter: null);
            };

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }
    }
}