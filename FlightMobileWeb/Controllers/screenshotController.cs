using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class screenshotController : ControllerBase
    {
        // GET: /screenshot
        [HttpGet]
        [Route("/screenshot/")]
        public IActionResult Get()
        {
            string someUrl = "http://localhost:" + Global.HttpPortprop.ToString() + "/screenshot";
            byte[] imageBytes = null;
            try
            {

                //     string someUrl = "http://localhost:5000/screenshot";  //simulator url
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    imageBytes = webClient.DownloadData(someUrl);
                }
                return File(imageBytes, "image/jpeg");
            } catch
            {
                return StatusCode(418);
            }
        }
    }
}
