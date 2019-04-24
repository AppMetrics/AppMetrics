// <copyright file="MetricsController.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace MetricsReceiveSanboxApi.Controllers
{
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] MetricsModel metrics)
        {
        }
    }
}