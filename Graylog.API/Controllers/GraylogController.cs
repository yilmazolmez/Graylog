using Microsoft.AspNetCore.Mvc;

namespace Graylog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GraylogController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok();
        }

    }
}
