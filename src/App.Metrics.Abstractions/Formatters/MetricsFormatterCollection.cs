// <copyright file="MetricsFormatterCollection.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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

        public IMetricsOutputFormatter GetType<T>()
            where T : IMetricsOutputFormatter
        {
            return GetType(typeof(T));
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

            return default(IMetricsOutputFormatter);
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

            return default(IMetricsOutputFormatter);
        }

        public void RemoveType<T>()
            where T : IMetricsOutputFormatter
        {
            RemoveType(typeof(T));
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
    }
}