using System;
using System.Collections.Generic;

namespace App.Metrics.Sandbox.JustForTesting
{
    public class RandomStatusCodeForTesting
    {
        private static readonly Random Rnd = new Random();
        private static readonly List<int> StatusCode = new List<int> { 200, 401, 401, 404, 403, 500, 500, 500 };

        public int NextStatusCode => StatusCode[Rnd.Next(0, StatusCode.Count - 1)];
    }
}