// <copyright file="RandomValuesForTesting.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace GrafanaCloudHostedMetricsSandboxMvc.Controllers
{
    public class RandomValuesForTesting
    {
        private static readonly List<Exception> Exceptions =
            new List<Exception> { new ArgumentException(), new ArithmeticException(), new InvalidCastException(), new NullReferenceException() };

        private static readonly Random Rnd = new Random();
        private static readonly List<int> StatusCodes = new List<int> { 200, 401, 401, 404, 403, 500, 500, 500 };

        public Func<Exception> NextException => () => Exceptions[Rnd.Next(0, Exceptions.Count - 1)];

        public Func<int> NextStatusCode => () => StatusCodes[Rnd.Next(0, StatusCodes.Count)];
    }
}