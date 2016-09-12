using Microsoft.AspNetCore.Mvc;

namespace ServiceOne.Controllers
{
    public class PingController : Controller
    {
        [Route("api/ping")]
        public IActionResult Ping()
        {
            if (Program.BadMode)
            {
                ServiceOneEventSource.Current.Log("Returning bad request.");
                return BadRequest();
            }

            return Ok();
        }

        [Route("api/good")]
        public IActionResult Good()
        {
            ServiceOneEventSource.Current.Log($"Enabling Good Mode on {Program.NodeName} for ServiceOne");

            Program.BadMode = false;

            return Ok();
        }

        [Route("api/bad")]
        public IActionResult Bad()
        {
            ServiceOneEventSource.Current.Log($"Enabling Bad Mode to on {Program.NodeName} for ServiceOne");

            Program.BadMode = true;

            return Ok();
        }
    }
}
