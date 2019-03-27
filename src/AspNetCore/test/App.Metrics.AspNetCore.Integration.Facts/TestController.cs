// <copyright file="TestController.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Metrics.AspNetCore.Integration.Facts
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IMetrics _metrics;

        public TestController(IMetrics metrics) { _metrics = metrics; }

        [HttpGet("400")]
        public IActionResult Bad()
        {
            return StatusCode(400);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("oauth/file/{clientid}")]
        public IActionResult File(IFormFile file)
        {
            return Created(new Uri("http://localhost"), 0);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        [HttpGet("exception")]
        public IActionResult GetException()
        {
            throw new Exception("test exception");
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("300ms")]
        // ReSharper disable InconsistentNaming
        public async Task<IActionResult> Get300ms()
            // ReSharper restore InconsistentNaming
        {
            _metrics.Clock.Advance(TimeUnit.Milliseconds, 300);
            await Task.FromResult(0);
            return StatusCode(200);
        }

        [HttpGet("30ms")]
        // ReSharper disable InconsistentNaming
        public async Task<IActionResult> Get30ms()
            // ReSharper restore InconsistentNaming
        {
            _metrics.Clock.Advance(TimeUnit.Milliseconds, 30);
            await Task.FromResult(0);
            return StatusCode(200);
        }

        [HttpGet("bad")]
        public IActionResult Get400()
        {
            return StatusCode(400);
        }

        [HttpGet("unauth")]
        public IActionResult Get401()
        {
            return StatusCode(401);
        }

        [HttpGet("error")]
        public IActionResult Get500()
        {
            return StatusCode(500);
        }

        [HttpGet("error-random/{passorfail}")]
        public async Task<IActionResult> GetRandomError([FromRoute] string passorfail)
        {
            await Task.Delay(10);
            return StatusCode(passorfail == "fail" ? 500 : 200);
        }

        [HttpGet("ignore")]
        public IActionResult Ignore() { return StatusCode(200); }

        [HttpGet("oauth/{clientid}")]
        public IActionResult OAuth(string clientId)
        {
            _metrics.Clock.Advance(TimeUnit.Milliseconds, 300);
            return StatusCode(200);
        }

        [HttpGet("oauth/error/{clientid}")]
        public IActionResult OAuthError(string clientId)
        {
            _metrics.Clock.Advance(TimeUnit.Milliseconds, 300);
            return StatusCode(500);
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("oauth/file/{clientid}")]
        public IActionResult Put(IFormFile file)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpGet("401")]
        public IActionResult Unauth()
        {
            return StatusCode(401);
        }
    }
}