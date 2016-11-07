using App.Metrics.Utils;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Utils
{
    public class EWMATests
    {
        [Fact]
        public void a_fifteen_minute_ewma_with_a_value_of_three()
        {
            var ewma = EWMA.FifteenMinuteEWMA();
            ewma.Update(3L);
            ewma.Tick();

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.6, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.56130419, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.52510399, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.49123845, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.45955700, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.42991879, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.40219203, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.37625345, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.35198773, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.32928698, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.30805027, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.28818318, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.26959738, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.25221023, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.23594443, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.22072766, 0.000001);
        }

        [Fact]
        public void ewma_a_five_minute_ewma_with_a_value_of_three()
        {
            var ewma = EWMA.FiveMinuteEWMA();
            ewma.Update(3L);
            ewma.Tick();

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.6, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.49123845, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.40219203, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.32928698, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.26959738, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.22072766, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.18071653, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.14795818, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.12113791, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.09917933, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.08120117, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.06648190, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.05443077, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.04456415, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.03648604, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.02987224, 0.000001);
        }

        [Fact]
        public void a_one_minute_ewma_with_a_value_of_three()
        {
            var ewma = EWMA.OneMinuteEWMA();
            ewma.Update(3L);
            ewma.Tick();

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.6, 0.000001);
            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.22072766, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.08120117, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.02987224, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.01098938, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.00404277, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.00148725, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.00054713, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.00020128, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.00007405, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.00002724, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.00001002, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.00000369, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.00000136, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.00000050, 0.000001);

            ElapseMinute(ewma);

            ewma.GetRate(TimeUnit.Seconds).Should().BeApproximately(0.00000018, 0.000001);
        }

        private void ElapseMinute(EWMA ewma)
        {
            for (var i = 1; i <= 12; i++)
            {
                ewma.Tick();
            }
        }
    }
}