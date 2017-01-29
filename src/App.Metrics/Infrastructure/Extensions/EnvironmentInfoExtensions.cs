// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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