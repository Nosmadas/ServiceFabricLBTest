using Microsoft.AspNetCore.Mvc;

namespace ServiceOne.Controllers
{
    public class PingController : Controller
    {
        private bool BadMode = false;

        [Route("api/ping")]
        public IActionResult Ping()
        {
            if (BadMode)
                return BadRequest();

            return Ok();
        }

        [Route("api/bad")]
        public IActionResult Toggle()
        {
            BadMode = !BadMode;
            return Ok();
        }
    }
}
