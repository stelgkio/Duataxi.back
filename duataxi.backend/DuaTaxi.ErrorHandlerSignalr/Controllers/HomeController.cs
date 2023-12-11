using Microsoft.AspNetCore.Mvc;

namespace DuaTaxi.Services.Signalr.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("DShop SignalR Service");
    }
}