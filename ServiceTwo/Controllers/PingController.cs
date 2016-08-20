using Microsoft.AspNetCore.Mvc;

namespace ServiceTwo.Controllers
{
    public class PingController : Controller
    {
        private bool BadMode = false;

        [Route("api/ping")]
        public IActionResult Ping()
        {
            ServiceTwoEventSource.Current.Log($"Received ping request, bad mode is set to {BadMode}");

            if (BadMode)
                return BadRequest();

            return Ok();
        }

        [Route("api/good")]
        public IActionResult Good()
        {
            ServiceTwoEventSource.Current.Log($"Enabling Good Mode on {Program.NodeName} for ServiceTwo");

            BadMode = false;
            return Ok();
        }

        [Route("api/bad")]
        public IActionResult Bad()
        {
            ServiceTwoEventSource.Current.Log($"Enabling Bad Mode to on {Program.NodeName} for ServiceTwo");

            BadMode = true;
            return Ok();
        }
    }
}
