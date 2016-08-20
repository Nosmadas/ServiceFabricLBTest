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
            ServiceOneEventSource.Current.Log($"Setting bad mode to {!BadMode} on {Program.NodeName} for ServiceOne");

            BadMode = !BadMode;
            return Ok();
        }
    }
}
