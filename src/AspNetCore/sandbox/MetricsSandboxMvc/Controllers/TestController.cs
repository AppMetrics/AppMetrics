// <copyright file="TestController.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace MetricsSandboxMvc.Controllers
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("params/{test}/{num}")]
        public IActionResult GetParams(string test, int num)
        {
            return Ok();
        }
    }
}