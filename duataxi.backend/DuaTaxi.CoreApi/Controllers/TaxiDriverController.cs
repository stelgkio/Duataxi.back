using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuaTaxi.Common;
using DuaTaxi.Common.Mvc;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Common.RestEase;
using DuaTaxi.Common.Types;
using DuaTaxi.Common.WebApiClient;
using DuaTaxi.CoreApi.Messages.Commands.TaxiDriver;
using DuaTaxi.CoreApi.Models.TaxiApi;
using DuaTaxi.CoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTracing;

namespace DuaTaxi.CoreApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TaxiDriverController : BaseController
    {
        private ITaxiService _taxiService;
        protected IOptions<RestEaseOptions> RestEaseConfig;
        private ILogger<TaxiDriverController> _logger;
        WebApiClient wac;
        public TaxiDriverController(ILogger<TaxiDriverController> _logger, IOptions<RestEaseOptions> RestEaseConfig, IBusPublisher busPublisher, ITaxiService taxiService, ITracer tracer) : base(busPublisher, tracer)
        {
            _taxiService = taxiService;
            this.RestEaseConfig = RestEaseConfig;
            this._logger = _logger;
        }
        public IActionResult Index()
        {
            return Ok();
        }
        // stelnw ena message edw 
        [HttpPost]
        public async Task<IActionResult> Post(CreateTaxiDriver command)
            => await SendAsync(command.BindId(c => c.Id), resourceId: command.Id.ToGuid(), resource: "taxiapi");


        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<object>> Get(Guid Id)
            => await _taxiService.GetAsync(Id);

        [Route("OnInit")]
        [HttpPost]
        public async Task<IActionResult> OnInit([FromBody] InitializeTaxiDriver initializeTaxi)
        {
            wac = new WebApiClient(RestEaseConfig, "taxiapi-service");
            try {

                var data = await wac.LoadAsync<TaxiDriverStatus>("TaxiDriver/TaxiDriverStatus", initializeTaxi.CustomerId);
                
                return Ok(data);


            } catch (Exception ex) {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                throw new Exception(ex.Message, ex);
            }

        }

        [Route("Activation")]
        [HttpPost]
        public async Task<IActionResult> Activation([FromBody] TaxiDriverActivation activation)
        {
            wac = new WebApiClient(RestEaseConfig, "taxiapi-service");
            try {
                //   var x = HttpContext.Request.Headers["Authorization"];

                var data = await wac.PostAsync<TaxiDriverActivation>("TaxiDriver/Activation", activation);

                return Ok(data);

            } catch (Exception ex) {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                throw new Exception(ex.Message, ex);
            }

        }   

        [Route("MapPositions")]
        [HttpPost]
        public async Task<IActionResult> MapPosition([FromBody] TaxiDriverActivation activation)
     => await SendAsync(activation.BindId(c => c.Id), resourceId: activation.Id.ToGuid(), resource: "taxiapi");

        [Route("DeActivate/{CustomerId}")]
        [HttpGet()]
        public async Task<bool> DeActivate(string CustomerId)
        {
            wac = new WebApiClient(RestEaseConfig, "taxiapi-service");
            try {
                //   var x = HttpContext.Request.Headers["Authorization"];

                var data = await wac.LoadAsync<bool>("TaxiDriver/DeActivate", CustomerId);

                return data;
            } catch (Exception ex) {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                throw new Exception(ex.Message, ex);
            }

        }

        [Route("ReActivate/{CustomerId}")]
        [HttpGet()]
        public async Task<bool> ReActivate(string CustomerId)
        {
            wac = new WebApiClient(RestEaseConfig, "taxiapi-service");
            try {

                var data = await wac.LoadAsync<bool>("TaxiDriver/ReActivate", CustomerId);

                return data;
            } catch (Exception ex) {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                throw new Exception(ex.Message, ex);
            }

        }

    }
}