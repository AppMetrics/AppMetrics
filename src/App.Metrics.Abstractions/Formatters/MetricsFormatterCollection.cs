// <copyright file="MetricsFormatterCollection.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.Metrics.Formatters
{
    public class MetricsFormatterCollection : Collection<IMetricsOutputFormatter>
    {
        public MetricsFormatterCollection() { }

        public MetricsFormatterCollection(IList<IMetricsOutputFormatter> list)
            : base(list)
        {
        }

        public IMetricsOutputFormatter GetType<TFormatter>()
            where TFormatter : IMetricsOutputFormatter
        {
            return GetType(typeof(TFormatter));
        }

        public IMetricsOutputFormatter GetType(Type formatterType)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var formatter = this[i];
                if (formatter.GetType() == formatterType)
                {
                    return formatter;
                }
            }

            return default;
        }

        public IMetricsOutputFormatter GetType(MetricsMediaTypeValue mediaTypeValue)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var formatter = this[i];
                if (formatter.MediaType == mediaTypeValue)
                {
                    return formatter;
                }
            }

            return default;
        }

        public void RemoveType<TFormatter>()
            where TFormatter : IMetricsOutputFormatter
        {
            RemoveType(typeof(TFormatter));
        }

        public void RemoveType(Type formatterType)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var formatter = this[i];
                if (formatter.GetType() == formatterType)
                {
                    RemoveAt(i);
                }
            }
        }

        public void RemoveType(MetricsMediaTypeValue mediaTypeValue)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var formatter = this[i];
                if (formatter.MediaType == mediaTypeValue)
                {
                    RemoveAt(i);
                }
            }
        }

        public void TryAdd<TFormatter>(IMetricsOutputFormatter formatter)
            where TFormatter : IMetricsOutputFormatter
        {
            RemoveType<TFormatter>();
            Add(formatter);
        }

        public void TryAdd(IMetricsOutputFormatter formatter)
        {
            RemoveType(formatter.GetType());
            Add(formatter);
        }
    }
}