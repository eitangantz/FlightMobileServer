using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class commandController : ControllerBase
    {
        // POST: api/command
        [HttpPost]
        public IActionResult Post(Command command)
        {
            try
            {
                Global.flightGearClientprop.connect(Global.IPprop.ToString(), Global.telnetPortprop);
                Global.flightGearClientprop.Execute(command);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(418);
            }
        }
    }
}