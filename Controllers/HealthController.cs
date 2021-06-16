using Microsoft.AspNetCore.Mvc;

namespace CodeExpBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController: ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Health()
        {
            return Ok();
        }
    }
}