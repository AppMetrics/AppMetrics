// <copyright file="RandomStatusCodeController.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using MetricsReportingSandboxMvc.JustForTesting;
using Microsoft.AspNetCore.Mvc;

namespace MetricsReportingSandboxMvc.Controllers
{
    [Route("api/[controller]")]
    public class RandomStatusCodeController : ControllerBase
    {
        private readonly RandomValuesForTesting _randomValuesForTesting;

        public RandomStatusCodeController(RandomValuesForTesting randomValuesForTesting)
        {
            _randomValuesForTesting = randomValuesForTesting;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(_randomValuesForTesting.NextStatusCode());
        }
    }
}
