using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.Core.Extensions;
using App.Metrics.Core.Internal;
using App.Metrics.Reporting;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core.Extensions
{
    // ReSharper disable InconsistentNaming
    public class Apdex_MetricValueExtensionsTests
        // ReSharper restore InconsistentNaming
    {
        [Fact]
        public void apdex_can_use_custom_data_keys_and_should_provide_corresponding_values()
        {
            // Arrange
            var value = new ApdexValue(1, 2, 3, 4, 5);
            var data = new Dictionary<string, object>();
            var customDataKeys = new Dictionary<ApdexValueDataKeys, string>
                                 {
                                     { ApdexValueDataKeys.Samples, "size_of_sample" }
                                 };

            // Act
            value.AddApdexValues(data, customDataKeys);

            // Assert
            data.ContainsKey(Constants.DataKeyMapping.Apdex[ApdexValueDataKeys.Samples]).Should().BeFalse();
            data["size_of_sample"].Should().Be(5);
        }

        [Fact]
        public void apdex_default_data_keys_should_provide_corresponding_values()
        {
            // Arrange
            var value = new ApdexValue(1, 2, 3, 4, 5);
            var data = new Dictionary<string, object>();

            // Act
            value.AddApdexValues(data);

            // Assert
            data[Constants.DataKeyMapping.Apdex[ApdexValueDataKeys.Score]].Should().Be(1.0);
            data[Constants.DataKeyMapping.Apdex[ApdexValueDataKeys.Satisfied]].Should().Be(2);
            data[Constants.DataKeyMapping.Apdex[ApdexValueDataKeys.Tolerating]].Should().Be(3);
            data[Constants.DataKeyMapping.Apdex[ApdexValueDataKeys.Frustrating]].Should().Be(4);
            data[Constants.DataKeyMapping.Apdex[ApdexValueDataKeys.Samples]].Should().Be(5);
        }
    }
}