// <copyright file="GraphiteSettingsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Reporting.Graphite.Client;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.Graphite.Facts
{
    // ReSharper disable InconsistentNaming
    public class GraphiteSettingsTests
        // ReSharper restore InconsistentNaming
    {
        [Fact]
        public void Base_address_cannot_be_null()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var settings = new GraphiteOptions(null);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("net.tcp://localhost", Protocol.Tcp)]
        [InlineData("net.udp://localhost", Protocol.Udp)]
        [InlineData("net.pickled://localhost", Protocol.Pickled)]
        public void Can_determine_protocol(string address, Protocol expected)
        {
            var settings = new GraphiteOptions(new Uri(address));

            settings.Protocol.Should().Be(expected);
        }
    }
}