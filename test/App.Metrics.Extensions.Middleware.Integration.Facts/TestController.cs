// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App.Metrics.Extensions.Middleware.Integration.Facts
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IMetrics _metrics;

        public TestController(IMetrics metrics) { _metrics = metrics; }

        [HttpGet("400")]
        public IActionResult BadRequest()
        {
            return StatusCode(400);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
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
        public IEnumerable<string> Ignore()
        {
            return new[] { "value1", "value2" };
        }

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

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpGet("401")]
        public IActionResult Unauthorized()
        {
            return StatusCode(401);
        }
    }
}