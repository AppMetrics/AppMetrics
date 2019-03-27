// <copyright file="FileController.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrafanaCloudHostedMetricsSandboxMvc.JustForTesting
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        [HttpPost]
        public IActionResult Post(IFormFile file)
        {
            return Created(new Uri("http://localhost"), 0);
        }

        [HttpPut]
        public IActionResult Put(IFormFile file)
        {
            return Ok();
        }
    }
}
