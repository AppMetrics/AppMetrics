using App.Metrics.Sandbox.JustForTesting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Metrics.Sandbox.Controllers
{
    [Route("api/[controller]")]
    public class ExceptionThrowingController : Controller
    {
        private readonly IMetrics _metrics;

        public ExceptionThrowingController(IMetrics metrics)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        }

        [HttpGet]
        public async Task<int> Get()
        {
            await Task.Run(() =>
            {
                throw new Exception();
            });

            return 0;
        }
    }
}
