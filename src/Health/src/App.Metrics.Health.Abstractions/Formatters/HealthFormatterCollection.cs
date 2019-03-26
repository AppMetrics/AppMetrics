// <copyright file="HealthFormatterCollection.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.Metrics.Health.Formatters
{
    public class HealthFormatterCollection : Collection<IHealthOutputFormatter>
    {
        public HealthFormatterCollection() { }

        public HealthFormatterCollection(IList<IHealthOutputFormatter> list)
            : base(list)
        {
        }

        public IHealthOutputFormatter GetType<T>()
            where T : IHealthOutputFormatter
        {
            return GetType(typeof(T));
        }

        public IHealthOutputFormatter GetType(Type formatterType)
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

        public IHealthOutputFormatter GetType(HealthMediaTypeValue mediaTypeValue)
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

        public void RemoveType<T>()
            where T : IHealthOutputFormatter
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

        public void RemoveType(HealthMediaTypeValue mediaTypeValue)
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

        public void TryAdd<TFormatter>(IHealthOutputFormatter formatter)
            where TFormatter : IHealthOutputFormatter
        {
            RemoveType<TFormatter>();
            Add(formatter);
        }

        public void TryAdd(IHealthOutputFormatter formatter)
        {
            RemoveType(formatter.GetType());
            Add(formatter);
        }
    }
}