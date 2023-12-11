using System.Threading.Tasks;
using DuaTaxi.Common;
using DuaTaxi.Common.Mvc;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.CoreApi.Messages.Commands.SMTP;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace DuaTaxi.CoreApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContactController : BaseController
    {
        

        public ContactController(IBusPublisher busPublisher,  ITracer tracer) : base(busPublisher, tracer)
        {
          
        }

        [HttpPost]       
        public async Task<IActionResult> Post([FromBody]Contact command)
          => await SendAsync(command.BindId(c => c.Id), resourceId: command.Id.ToGuid(), resource: "smtpApi");
    }
}