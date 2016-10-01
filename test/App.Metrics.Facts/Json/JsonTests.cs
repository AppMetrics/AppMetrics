using App.Metrics.Json;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Json
{
    public class JsonTests
    {
        [Fact]
        public void Json_DoubleNanIsSerialziedCorrectly()
        {
            new DoubleJsonValue(double.NaN).AsJson().Should().Be("null");
        }
    }
}