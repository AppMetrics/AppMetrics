using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Api.Sample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("500")]
        public IActionResult Get500()
        {
            return StatusCode(500);
        }

        [HttpGet("401")]
        public IActionResult Get401()
        {
            return StatusCode(401);
        }

        [HttpGet("400")]
        public IActionResult Get400()
        {
            return StatusCode(400);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}