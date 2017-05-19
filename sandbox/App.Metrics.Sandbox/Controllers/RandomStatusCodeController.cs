using System;
using App.Metrics.Sandbox.JustForTesting;
using Microsoft.AspNetCore.Mvc;

namespace App.Metrics.Sandbox.Controllers
{
    [Route("api/[controller]")]
    public class RandomStatusCodeController : Controller
    {
        private readonly RandomValuesForTesting _randomValuesForTesting;
        private readonly IMetrics _metrics;

        public RandomStatusCodeController(IMetrics metrics, RandomValuesForTesting randomValuesForTesting)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _randomValuesForTesting = randomValuesForTesting;
        }

        [HttpGet]
        public IActionResult Get()
        {            
            return StatusCode(_randomValuesForTesting.NextStatusCode());
        }
    }
}
