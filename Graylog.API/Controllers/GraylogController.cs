using Graylog.Application;
using Microsoft.AspNetCore.Mvc;

namespace Graylog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GraylogController : ControllerBase
    {
        private readonly IGraylogService _graylogService;

        public GraylogController(IGraylogService graylogService)
        {
            _graylogService = graylogService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _graylogService.GetAsync();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string requestMessage)
        {
            var response = await _graylogService.PostAsync(requestMessage);

            return Ok(response);
        }

    }
}
