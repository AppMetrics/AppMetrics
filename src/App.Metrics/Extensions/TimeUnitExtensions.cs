using System;

// ReSharper disable CheckNamespace

namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class TimeUnitExtensions
    {
        private static readonly long[,] conversionFactors = BuildConversionFactorsMatrix();

        public static long Convert(this TimeUnit sourceUnit, TimeUnit targetUnit, long value)
        {
            if (sourceUnit == targetUnit)
            {
                return value;
            }

            return System.Convert.ToInt64(value * sourceUnit.ScalingFactorFor(targetUnit));
        }

        public static TimeUnit FromUnit(string unit)
        {
            switch (unit)
            {
                case "ns":
                    return TimeUnit.Nanoseconds;
                case "us":
                    return TimeUnit.Microseconds;
                case "ms":
                    return TimeUnit.Milliseconds;
                case "s":
                    return TimeUnit.Seconds;
                case "min":
                    return TimeUnit.Minutes;
                case "h":
                    return TimeUnit.Hours;
                case "days":
                    return TimeUnit.Days;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit));
            }
        }

        public static double ScalingFactorFor(this TimeUnit sourceUnit, TimeUnit targetUnit)
        {
            if (sourceUnit == targetUnit)
            {
                return 1.0;
            }

            var sourceIndex = (int)sourceUnit;
            var targetIndex = (int)targetUnit;

            if (sourceIndex < targetIndex)
            {
                return 1 / (double)conversionFactors[targetIndex, sourceIndex];
            }
            else
            {
                return conversionFactors[sourceIndex, targetIndex];
            }
        }

        public static long ToDays(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Days, value);
        }

        public static long ToHours(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Hours, value);
        }

        public static long ToMicroseconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Microseconds, value);
        }

        public static long ToMilliseconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Milliseconds, value);
        }

        public static long ToMinutes(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Minutes, value);
        }

        public static long ToNanoseconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Nanoseconds, value);
        }

        public static long ToSeconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Seconds, value);
        }

        public static string Unit(this TimeUnit unit)
        {
            switch (unit)
            {
                case TimeUnit.Nanoseconds:
                    return "ns";
                case TimeUnit.Microseconds:
                    return "us";
                case TimeUnit.Milliseconds:
                    return "ms";
                case TimeUnit.Seconds:
                    return "s";
                case TimeUnit.Minutes:
                    return "min";
                case TimeUnit.Hours:
                    return "h";
                case TimeUnit.Days:
                    return "days";
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit));
            }
        }

        private static long[,] BuildConversionFactorsMatrix()
        {
            var count = Enum.GetValues(typeof(TimeUnit)).Length;

            var matrix = new long[count, count];
            var timingFactors = new[]
            {
                1000L, // Nanoseconds to microseconds
                1000L, // Microseconds to milliseconds
                1000L, // Milliseconds to seconds
                60L, // Seconds to minutes
                60L, // Minutes to hours
                24L // Hours to days
            };

            for (var source = 0; source < count; source++)
            {
                var cumulativeFactor = 1L;
                for (var target = source - 1; target >= 0; target--)
                {
                    cumulativeFactor *= timingFactors[target];
                    matrix[source, target] = cumulativeFactor;
                }
            }
            return matrix;
        }
    }
}