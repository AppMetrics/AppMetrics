﻿// <copyright file="RandomExceptionController.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using MetricsInfluxDBSandboxMvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MetricsInfluxDBSandboxMvc.JustForTesting
{
    [Route("api/[controller]")]
    public class RandomExceptionController : Controller
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