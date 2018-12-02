// <copyright file="MetricsReservoirSamplingBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Facts.TestHelpers;
using App.Metrics.Internal;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class MetricsReservoirSamplingBuilderTests
    {
        [Fact]
        public void Can_set_default_reservoir_to_algorithmr()
        {
            // Arrange
            DefaultMetricsBuilderFactory metricsBuilder;

            // Act
            var metrics = new MetricsBuilder().SampleWith.AlgorithmR().Build();
            metricsBuilder = metrics.Build as DefaultMetricsBuilderFactory;
            var reservoir = metricsBuilder?.DefaultSamplingReservoir.Instance();

            // Assert
            reservoir.Should().BeOfType<DefaultAlgorithmRReservoir>();
        }

        [Fact]
        public void Can_set_default_reservoir_to_custom_func_reservoir()
        {
            // Arrange
            DefaultMetricsBuilderFactory metricsBuilder;

            // Act
            var metrics = new MetricsBuilder().SampleWith.Reservoir(() => new CustomReservoir()).Build();
            metricsBuilder = metrics.Build as DefaultMetricsBuilderFactory;
            var reservoir = metricsBuilder?.DefaultSamplingReservoir.Instance();

            // Assert
            reservoir.Should().BeOfType<CustomReservoir>();
        }

        [Fact]
        public void Can_set_default_reservoir_to_custom_implementation()
        {
            // Arrange
            DefaultMetricsBuilderFactory metricsBuilder;

            // Act
            var metrics = new MetricsBuilder().SampleWith.Reservoir<CustomReservoir>().Build();
            metricsBuilder = metrics.Build as DefaultMetricsBuilderFactory;
            var reservoir = metricsBuilder?.DefaultSamplingReservoir.Instance();

            // Assert
            reservoir.Should().BeOfType<CustomReservoir>();
        }

        [Fact]
        public void Can_set_default_reservoir_to_forward_decaying_should_the_last_selected_reservoir()
        {
            // Arrange
            DefaultMetricsBuilderFactory metricsBuilder;

            // Act
            var metrics = new MetricsBuilder().SampleWith.SlidingWindow().SampleWith.ForwardDecaying().Build();
            metricsBuilder = metrics.Build as DefaultMetricsBuilderFactory;
            var reservoir = metricsBuilder?.DefaultSamplingReservoir.Instance();

            // Assert
            reservoir.Should().BeOfType<DefaultForwardDecayingReservoir>();
        }

        [Fact]
        public void Can_set_default_reservoir_to_sliding_window()
        {
            // Arrange
            DefaultMetricsBuilderFactory metricsBuilder;

            // Act
            var metrics = new MetricsBuilder().SampleWith.SlidingWindow().Build();
            metricsBuilder = metrics.Build as DefaultMetricsBuilderFactory;
            var reservoir = metricsBuilder?.DefaultSamplingReservoir.Instance();

            // Assert
            reservoir.Should().BeOfType<DefaultSlidingWindowReservoir>();
        }

        [Fact]
        public void Default_reservoir_is_forward_decaying()
        {
            // Arrange
            DefaultMetricsBuilderFactory metricsBuilder;

            // Act
            var metrics = new MetricsBuilder().Build();
            metricsBuilder = metrics.Build as DefaultMetricsBuilderFactory;
            var reservoir = metricsBuilder?.DefaultSamplingReservoir.Instance();

            // Assert
            reservoir.Should().BeOfType<DefaultForwardDecayingReservoir>();
        }
    }
}