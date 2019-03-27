// <copyright file="RandomExceptionController.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using MetricsSandboxMvc.JustForTesting;
using Microsoft.AspNetCore.Mvc;

namespace MetricsSandboxMvc.Controllers
{
    [Route("api/[controller]")]
    public class RandomExceptionController : ControllerBase
    {
        private readonly RandomValuesForTesting _randomValuesForTesting;

        public RandomExceptionController(RandomValuesForTesting randomValuesForTesting)
        {
            _randomValuesForTesting = randomValuesForTesting;
        }

        [HttpGet]
        public void Get()
        {
            throw _randomValuesForTesting.NextException();
        }
    }
}