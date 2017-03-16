using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace App.Metrics.Sandbox.Controllers
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
