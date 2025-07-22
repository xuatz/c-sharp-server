using Microsoft.AspNetCore.Mvc;

namespace CSharpServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello World!" });
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            return Ok(new { message = $"Hello {name}!" });
        }
    }
}