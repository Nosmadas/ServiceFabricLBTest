using Microsoft.AspNetCore.Mvc;

namespace ServiceTwo.Controllers
{
    public class PingController : Controller
    {
        [Route("api/ping")]
        public IActionResult Ping()
        {
            if (Program.BadMode)
            {
                ServiceTwoEventSource.Current.Log($"Returning Bad request");

                return BadRequest();
            }

            return Ok();
        }

        [Route("api/good")]
        public IActionResult Good()
        {
            ServiceTwoEventSource.Current.Log($"Enabling Good Mode on {Program.NodeName} for ServiceTwo");

            Program.BadMode = false;

            return Ok();
        }

        [Route("api/bad")]
        public IActionResult Bad()
        {
            ServiceTwoEventSource.Current.Log($"Enabling Bad Mode to on {Program.NodeName} for ServiceTwo");

            Program.BadMode = true;

            return Ok();
        }
    }
}
