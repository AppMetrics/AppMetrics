// <copyright file="EnvFormatterCollection.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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

        public IEnvOutputFormatter GetType<T>()
            where T : IEnvOutputFormatter
        {
            return GetType(typeof(T));
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

            return default(IEnvOutputFormatter);
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

            return default(IEnvOutputFormatter);
        }

        public void RemoveType<T>()
            where T : IEnvOutputFormatter
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