using DuaTaxi.Common.Handlers;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Service.TaxiApi.Messages.Commands;
using DuaTaxi.Service.TaxiApi.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuaTaxi.Common.Types;
using DuaTaxi.Service.TaxiApi.Entities;
using DuaTaxi.Service.TaxiApi.Messages.Events;

namespace DuaTaxi.Service.TaxiApi.Handlers.TaxiDriver
{
    public class CreatTaxiDriverHandler : ICommandHandler<CreateTaxiDriver>
    {
        private readonly ITaxiDriverRepository _taxidriverRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<CreateTaxiDriver> _logger;


        public CreatTaxiDriverHandler(ITaxiDriverRepository taxidriverRepository,IBusPublisher busPublisher, ILogger<CreateTaxiDriver> logger)
        {
            _taxidriverRepository = taxidriverRepository;           
            _busPublisher = busPublisher;
            _logger = logger;
        }

        public async Task HandleAsync(CreateTaxiDriver command, ICorrelationContext context)
        {

            var customer = await _taxidriverRepository.GetCustomerAsync(command.CustomerId.ToString());
            if (customer is null)
            {
                //onError: -> publish CreateDiscountRejected
                //throw new DuaTaxiException("customer_not_found",
                //    $"Customer with id: '{command.CustomerId}' was not found.");

                _logger.LogWarning($"Customer with id: '{command.CustomerId}' was not found.");
                await _busPublisher.PublishAsync(new CreateTaxiDriverRejected(
                    command.CustomerId,
                    $"Customer with id: '{command.CustomerId}' was not found.",
                    "customer_not_found"),
                    context);

                return;
            }
            try
            {
                var discount = new TaxiDriverStatus( command.CustomerId.ToString(), command.ConnectionId, command.Status,"");
                await _taxidriverRepository.AddAsync(discount);
            }
            catch (Exception ex )
            {

                _logger.LogError($"TaxiDriver could not Created with error {ex.StackTrace}");
                await _busPublisher.PublishAsync(new CreateTaxiDriverRejected(
                   command.CustomerId,
                   $"Customer with id: '{command.CustomerId}' was not found.",
                   "customer_not_found"),
                   context);
                return;
            }

            // Unique code validation
          
            // kanw publish gia na enhmerwsw kapoio allo micro oti kati egine 
            // tha prepei na kanw subscribe ekei pou thelw na dw to message
            await _busPublisher.PublishAsync(new TaxiDriverCreated(command.Id, command.CustomerId), context);
        }
    }
}
