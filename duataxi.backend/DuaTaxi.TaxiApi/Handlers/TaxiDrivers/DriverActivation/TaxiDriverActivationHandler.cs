using DuaTaxi.Common.Handlers;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Service.TaxiApi.Repositories;
using DuaTaxi.Services.TaxiApi.Entities.Models;
using DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers.DriverActivation;
using DuaTaxi.Services.TaxiApi.Messages.Commands;
using DuaTaxi.Services.TaxiApi.Repositories.ActiveDriversRepo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers {
    public class TaxiDriverActivationHandler : ITaxiDriverActivationHandler
    {
        private readonly ITaxiDriverRepository _taxiDriverRepository;
        private readonly IActiveTaxiDriverRepository _activeTaxiDriverRepository;
        private readonly ILogger<TaxiDriverActivationHandler> _logger;

        public TaxiDriverActivationHandler(ITaxiDriverRepository taxiDriverRepository, IActiveTaxiDriverRepository activeTaxiDriverRepository, ILogger<TaxiDriverActivationHandler> logger) {
            _taxiDriverRepository = taxiDriverRepository;
            _activeTaxiDriverRepository = activeTaxiDriverRepository;
            _logger = logger;
        }
        public async Task<ActiveDrivers> HandleAsync(TaxiDriverActivation command) {

            var driver = await _taxiDriverRepository.GetCustomerAsync(command.CustomerId);
            if (!driver.Status) {
                _logger.LogError("Driver not exist", null);
                //await _busPublisher.PublishAsync(new InitializeTaxiDriverRejected(
                //    command.CustomerId.ToString(),
                //    $"Customer with id: '{command.CustomerId}' could not find ",
                //    "Initialize Rejected"),
                //    context);
                ////add to TaxiDriverStatus
                //await _taxidriverRepository.AddAsync(new TaxiDriverStatus(command.ConnectionId, command.CustomerId, false));
                return null;
            }

            var data = new ActiveDrivers(command.Id, command.CustomerId, command.Latitude, command.Longtitute, true);
            await _activeTaxiDriverRepository.AddAsync(data);

            return data;


        }
    }
}
