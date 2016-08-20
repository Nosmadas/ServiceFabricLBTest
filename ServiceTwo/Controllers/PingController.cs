using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ServiceTwo.Controllers
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
            ServiceOneEventSource.Current.Log($"Setting bad mode to {!BadMode} on {Program.NodeName}");

            BadMode = !BadMode;
            return Ok();
        }
    }
}
