using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Utils
{
    public class MetricUnitTests
    {
        [Theory]
        [
        InlineData("test", 0, TimeUnit.Seconds, "0.00 s"),
        InlineData("test", 1, TimeUnit.Nanoseconds, "1.00 ns"),
        InlineData("test", 1, TimeUnit.Microseconds, "1.00 us"),
        InlineData("test", 1, TimeUnit.Milliseconds, "1.00 ms"),
        InlineData("test", 1, TimeUnit.Minutes, "1.00 min"),
        InlineData("test", 1, TimeUnit.Hours, "1.00 h"),
        InlineData("test", 1, TimeUnit.Days, "1.00 days")
        ]
        public void Unit_CanFormatDuration(string unit, double value, TimeUnit timeUnit, string output)
        {
            Unit.Custom(unit).FormatDuration(value, timeUnit).Should().Be(output);
        }

        [Theory]
        [
        InlineData("test", 0, "0.00 test"),
        InlineData("test", 1, "1.00 test"),
        InlineData("test", 1.2, "1.20 test"),
        InlineData("test", 1.111, "1.11 test"),
        InlineData("test", 1.119, "1.12 test")
        ]
        public void Unit_CanFormatDurationWithoutTimeUnit(string unit, double value, string output)
        {
            Unit.Custom(unit).FormatDuration(value, null).Should().Be(output);
        }

        [Theory]
        [
        InlineData("test", 0, TimeUnit.Seconds, "0.00 test/s"),
        InlineData("test", 1, TimeUnit.Nanoseconds, "1.00 test/ns"),
        InlineData("test", 1, TimeUnit.Microseconds, "1.00 test/us"),
        InlineData("test", 1, TimeUnit.Milliseconds, "1.00 test/ms"),
        InlineData("test", 1, TimeUnit.Minutes, "1.00 test/min"),
        InlineData("test", 1, TimeUnit.Hours, "1.00 test/h"),
        InlineData("test", 1, TimeUnit.Days, "1.00 test/days")
        ]
        public void Unit_CanFormatRate(string unit, double value, TimeUnit timeUnit, string output)
        {
            Unit.Custom(unit).FormatRate(value, timeUnit).Should().Be(output);
        }
    }
}
