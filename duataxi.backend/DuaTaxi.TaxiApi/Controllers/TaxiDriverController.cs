using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuaTaxi.Common.Dispatchers;
using DuaTaxi.Common.Mvc;
using DuaTaxi.Common.Types;
using DuaTaxi.Common.WebApiClient;
using DuaTaxi.Service.TaxiApi.Dto;
using DuaTaxi.Service.TaxiApi.Messages.Commands;
using DuaTaxi.Service.TaxiApi.Query;
using DuaTaxi.Service.TaxiApi.Repositories;
using DuaTaxi.Services.TaxiApi.Entities.DTO;
using DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers;
using DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers.DriverActivation;
using DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers.SetPickUp;
using DuaTaxi.Services.TaxiApi.Messages.Commands;
using DuaTaxi.Services.TaxiApi.Repositories.ActiveDriversRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DuaTaxi.Service.TaxiApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class TaxiDriverController : ControllerBase {
      
        private readonly IDispatcher _dispatcher;
        private readonly IActiveTaxiDriverRepository _activeTaxiDriverRepository;
        private readonly ITaxiDriverRepository _taxiDriverRepository;
        private readonly IInitializeTaxiDriverHandler _initializeTaxiDriver;
        private ILogger<TaxiDriverController> _logger;
        private readonly ISetPickUpHandler _setPickUpHandler;
        private readonly ITaxiDriverActivationHandler _taxiDriverActivation;


        public TaxiDriverController(IDispatcher dispatcher, IActiveTaxiDriverRepository activeTaxiDriverRepository, IInitializeTaxiDriverHandler initializeTaxiDriver, ITaxiDriverRepository taxiDriverRepository, ILogger<TaxiDriverController> logger,
            ISetPickUpHandler setPickUpHandler, ITaxiDriverActivationHandler taxiDriverActivation) {
            _dispatcher = dispatcher;
            _activeTaxiDriverRepository = activeTaxiDriverRepository;
            _taxiDriverRepository = taxiDriverRepository;
            _initializeTaxiDriver = initializeTaxiDriver;
            _logger = logger;
            _setPickUpHandler = setPickUpHandler;
            _taxiDriverActivation = taxiDriverActivation;
        }

        // Idempotent
        // No side effects
        // Doesn't mutate a state
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreateTaxiDriverDto>>> Get([FromQuery] BrowseTaxiDrive query)
            => Ok(await _dispatcher.QueryAsync(query));

        //[HttpGet("{id:guid}")]
        //public async Task<ActionResult<DiscountDetailsDto>> Get([FromRoute] GetDiscount query)
        //{
        //    var discount = await _dispatcher.QueryAsync(query);
        //    if (discount is null)
        //    {
        //        return NotFound();
        //    }

        //    return discount;
        //}

        [HttpPost]
        public async Task<ActionResult> Post(CreateTaxiDriver command) {
            await _dispatcher.SendAsync(command.BindId(c => c.Id));

            return Accepted();
        }


        [Route("DeActivate/{customerID}")]
        [HttpGet]
        public async Task<IActionResult> DeActivateDriver(string customerId) {
            try {
                var deactivate = await _activeTaxiDriverRepository.DeActivate(customerId);

                if (!deactivate.IsActive)
                    return Ok(true);

                return Ok(false);
            }
            catch (DuaTaxiException ex) {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }


        [Route("ReActivate/{customerID}")]
        [HttpGet]
        public async Task<IActionResult> ReActivateDriver(string customerId) {
            try {
                var deactivate = await _activeTaxiDriverRepository.ReActivate(customerId);

                if (deactivate.IsActive)
                    return Ok(true);

                return Ok(false);
            }
            catch (DuaTaxiException ex) {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }


        [Route("TaxiDriverStatus/{customerID}")]
        [HttpGet]
        public async Task<IActionResult> TaxiDriverStatus(string customerId)
        {
            try {
                var TaxiDriverStatus = await _taxiDriverRepository.GetCustomerAsync(customerId);

                if(TaxiDriverStatus is null ) {
                    TaxiDriverStatus =await _initializeTaxiDriver.HandleAsync(customerId);
                }

                return Ok(TaxiDriverStatus);


            } catch (DuaTaxiException ex) {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [Route("Activation")]
        [HttpPost]
        public async Task<IActionResult> TaxiDriverActivation([FromBody] TaxiDriverActivation taxiDriverActivation)
        {
            try {

                var driverData = await _taxiDriverActivation.HandleAsync(taxiDriverActivation);

                if (driverData is null)
                    return Ok(null);

                return Ok(driverData);

            } catch (DuaTaxiException ex) {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                return BadRequest(ex.Message);
            }           
        }                             


        [Route("SetPickup")]
        [HttpPost]
        public async Task<IActionResult> SetPickup([FromBody] SetPickUpDto setPickDto)
        {
            try {                                                                   
                var driverData = await _setPickUpHandler.HandleAsync(setPickDto);

                if (driverData is null)
                    return Ok(null);

                return Ok(driverData);

            } catch (DuaTaxiException ex) {
                _logger.LogError(ex.Message, ex.InnerException, ex.StackTrace);
                return BadRequest(ex.Message);
            }                                                 
        }
    }
}