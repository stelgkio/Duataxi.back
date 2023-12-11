using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuaTaxi.Service.Payments.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace DuaTaxi.Service.Payments.Controllers
{
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("DuaTaxi Payment Service");

       
    }
}