// <copyright file="EnvFormatterCollection.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.Metrics.Formatters
{
    public class EnvFormatterCollection : Collection<IEnvOutputFormatter>
    {
        public EnvFormatterCollection() { }

        public EnvFormatterCollection(IList<IEnvOutputFormatter> list)
            : base(list)
        {
        }

        public IEnvOutputFormatter GetType<TFormatter>()
            where TFormatter : IEnvOutputFormatter
        {
            return GetType(typeof(TFormatter));
        }

        public IEnvOutputFormatter GetType(Type formatterType)
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

        public IEnvOutputFormatter GetType(MetricsMediaTypeValue mediaTypeValue)
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
            where TFormatter : IEnvOutputFormatter
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

        public void TryAdd<TFormatter>(IEnvOutputFormatter formatter)
            where TFormatter : IEnvOutputFormatter
        {
            RemoveType<TFormatter>();
            Add(formatter);
        }

        public void TryAdd(IEnvOutputFormatter formatter)
        {
            RemoveType(formatter.GetType());
            Add(formatter);
        }
    }
}