// <copyright file="EnvironmentInfoExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace
namespace App.Metrics.Infrastructure
    // ReSharper restore CheckNamespace
{
    public static class EnvironmentInfoExtensions
    {
      public static IDictionary<string, string> ToEnvDictionary(this EnvironmentInfo environmentInfo)
        {
            return environmentInfo.Entries.ToDictionary(entry => entry.Name, entry => entry.Value);
        }
    }
}