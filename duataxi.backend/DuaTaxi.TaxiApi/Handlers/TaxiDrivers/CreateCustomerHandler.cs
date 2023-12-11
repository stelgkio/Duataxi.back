using DuaTaxi.Common.Handlers;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Services.TaxiApi.Entities.Models;
using DuaTaxi.Services.TaxiApi.Messages.Commands;
using DuaTaxi.Services.TaxiApi.Messages.Events.Customer;
using DuaTaxi.Services.TaxiApi.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers {
    public class CreateCustomerHandler : ICommandHandler<CreateTaxiDriverCustomer>, ICommandHandler<DeleteTaxiDriverCustomer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<Customer> _logger;

        public CreateCustomerHandler(ICustomerRepository customerRepository, IBusPublisher busPublisher, ILogger<Customer> logger) {
            _customerRepository = customerRepository;
            _busPublisher = busPublisher;
            _logger = logger;
        }
        public async Task HandleAsync(CreateTaxiDriverCustomer command, ICorrelationContext context) {
            try {
                var customerExist = await _customerRepository.GetAsync(command.Id);

                if (customerExist is null) {
                    var newCustomer = new Customer(command.CustomerId, command.Name, command.Email, command.PhoneNumber, command.Type);
                    await _customerRepository.AddAsync(newCustomer);
                }

            }
            catch (Exception ex) {

                _logger.LogError($"Customer could not Created with error {ex.StackTrace}");
                await _busPublisher.PublishAsync(new CreateCustomerRejected(
                   command.CustomerId,
                   $"Customer with id: '{command.CustomerId}' could not Created with error {ex.StackTrace}",
                   "Create TaxiDriverCustomer Exception"),
                   context);
                return;
            }

            return;
        }

        public async Task HandleAsync(DeleteTaxiDriverCustomer command, ICorrelationContext context)
        {
            try {
                var customerExist = await _customerRepository.GetAsync(command.CustomerId);

                if (customerExist != null) {                
                    await _customerRepository.DeleteAsync(command.CustomerId);
                }

            } catch (Exception ex) {

                _logger.LogError($"Customer with id: {command.CustomerId} could not Deleted with error {ex.StackTrace}");
               
                return;
            }

            return;
        }
    }
}
