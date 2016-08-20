using Microsoft.AspNetCore.Mvc;

namespace ServiceOne.Controllers
{
    public class PingController : Controller
    {
        private bool BadMode = false;

        [Route("api/ping")]
        public IActionResult Ping()
        {
            ServiceOneEventSource.Current.Log($"Received ping request, bad mode is set to {BadMode}");

            if (BadMode)
                return BadRequest();

            return Ok();
        }

        [Route("api/good")]
        public IActionResult Good()
        {
            ServiceOneEventSource.Current.Log($"Enabling Good Mode on {Program.NodeName} for ServiceOne");

            BadMode = false;
            return Ok();
        }

        [Route("api/bad")]
        public IActionResult Bad()
        {
            ServiceOneEventSource.Current.Log($"Enabling Bad Mode to on {Program.NodeName} for ServiceOne");

            BadMode = true;
            return Ok();
        }
    }
}
