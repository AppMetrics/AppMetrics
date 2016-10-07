// Written by Iulian Margarintescu
// 
// Ported to .NET Standard Library by Allan Hardy
// Original repo: https://github.com/etishor/Metrics.NET

using System.Collections.Generic;

namespace App.Metrics.Sampling
{
    public interface ISnapshot
    {
        long Count { get; }

        long Max { get; }

        string MaxUserValue { get; }

        double Mean { get; }

        double Median { get; }

        long Min { get; }

        string MinUserValue { get; }

        double Percentile75 { get; }

        double Percentile95 { get; }

        double Percentile98 { get; }

        double Percentile99 { get; }

        double Percentile999 { get; }

        int Size { get; }

        double StdDev { get; }

        IEnumerable<long> Values { get; }

        double GetValue(double quantile);
    }
}