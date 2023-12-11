using Microsoft.AspNetCore.Mvc;

namespace DuaTaxi.Services.Operations.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("DuaTaxi Operations Service");
    }
}