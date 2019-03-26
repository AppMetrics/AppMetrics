// <copyright file="EnvironmentInfoExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
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