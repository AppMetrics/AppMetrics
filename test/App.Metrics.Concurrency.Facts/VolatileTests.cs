using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class VolatileTests
    {
        [Fact]
        public void can_get_and_set_value()
        {
            var value = new VolatileDouble(1.5);

            value.GetValue().Should().Be(1.5);
            value.SetValue(2.3);
            value.GetValue().Should().Be(2.3);
        }

        [Fact]
        public void has_correct_size()
        {
            VolatileDouble.SizeInBytes.Should().Be(Marshal.SizeOf<VolatileDouble>());
        }

        [Fact]
        public void can_get_and_set_value_non_volatile()
        {
            var value = new VolatileDouble(1.5);

            value.NonVolatileGetValue().Should().Be(1.5);
            value.NonVolatileSetValue(2.3);
            value.NonVolatileGetValue().Should().Be(2.3);
        }
    }
}