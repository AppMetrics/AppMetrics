using System;
using App.Metrics.Sandbox.JustForTesting;
using Microsoft.AspNetCore.Mvc;

namespace App.Metrics.Sandbox.Controllers
{
    [Route("api/[controller]")]
    public class RandomStatusCodeController : Controller
    {
        private readonly RandomStatusCodeForTesting _randomStatusCodeForTesting;
        private readonly IMetrics _metrics;

        public RandomStatusCodeController(IMetrics metrics, RandomStatusCodeForTesting randomStatusCodeForTesting)
        {
            if (metrics == null)
            {
                throw new ArgumentNullException(nameof(metrics));
            }

            _metrics = metrics;
            _randomStatusCodeForTesting = randomStatusCodeForTesting;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(_randomStatusCodeForTesting.NextStatusCode);
        }
    }
}
