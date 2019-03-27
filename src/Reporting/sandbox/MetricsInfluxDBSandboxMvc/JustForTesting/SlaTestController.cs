// <copyright file="SlaTestController.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Mvc;

namespace MetricsInfluxDBSandboxMvc.JustForTesting
{
    [Route("api/[controller]")]
    public class SlaTestController : Controller
    {
        private static readonly Random Rnd = new Random();
        private readonly IMetrics _metrics;

        public SlaTestController(IMetrics metrics) { _metrics = metrics; }

        [HttpGet]
        [Route("timer")]
        public async Task<IActionResult> GetTimer()
        {
            using (_metrics.Measure.Timer.Time(SandboxMetricsRegistry.DatabaseTimer, new MetricTags("client_id", HttpContext.User.Claims.First(c => c.Type == "client_id").Value)))
            {
                await Task.Delay(Rnd.Next(350), HttpContext.RequestAborted);
            }

            return Ok();
        }
    }
}