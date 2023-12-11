using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Common.RestEase;
using DuaTaxi.Common.WebApiClient;
using DuaTaxi.CoreApi.Models.TaxiSetPick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTracing;

namespace DuaTaxi.CoreApi.Controllers
{
    public class ClientController : BaseController
    {
        protected IOptions<RestEaseOptions> RestEaseConfig;
        private ILogger<TaxiDriverController> _logger;
        WebApiClient wac;
        public ClientController(ILogger<TaxiDriverController> _logger, IOptions<RestEaseOptions> RestEaseConfig, IBusPublisher busPublisher, ITracer tracer) : base(busPublisher, tracer)
        {
            this.RestEaseConfig = RestEaseConfig;
            this._logger = _logger;
        }
        public IActionResult Index()
        {
            return Ok();
        }

        [Route("Taxi/SetPickup")]
        [HttpPost]
        public async Task<IActionResult> OnInit([FromBody] SetPickup setpickup)
        {
            wac = new WebApiClient(RestEaseConfig, "taxiapi-service");
            try {

                var data = await wac.PostAsync<SetPickup>("TaxiDriver/SetPickup",setpickup);

                return Ok(data);


            } catch (Exception ex) {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                throw new Exception(ex.Message, ex);
            }

        }
    }
}