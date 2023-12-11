using DuaTaxi.Common.Handlers;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Common.Types;
using DuaTaxi.Service.Payments.Entities.Models;
using DuaTaxi.Service.Payments.Messages.Commands;
using DuaTaxi.Service.Payments.Messages.Evends.Payment;
using DuaTaxi.Service.Payments.Repository;
using DuaTaxi.Service.Payments.Services.DayOfExpiration;
using DuaTaxi.Service.Payments.Services.FindExistingPayments;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.Payments.Handler {
    public class CreateFirstPaymentHandler : ICommandHandler<CreateFirstPayment> {
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<Payment> _logger;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IFindExistingPayments _findExistingPayments;
        private readonly IDayOfExpirationService _dayOfExpirationService;

        public CreateFirstPaymentHandler(IPaymentRepository paymentRepository, IBusPublisher busPublisher, ILogger<Payment> logger, IFindExistingPayments findExistingPayments, IDayOfExpirationService dayOfExpirationService) {
            _paymentRepository = paymentRepository;
            _findExistingPayments = findExistingPayments;
            _busPublisher = busPublisher;
            _logger = logger;
            _dayOfExpirationService = dayOfExpirationService;
        }
        public async Task HandleAsync(CreateFirstPayment command, ICorrelationContext context) {
            try {
                var checkPayment = await _findExistingPayments.CheckCustomerPayments(command.CustomerId);
                if (checkPayment) {
                    /// an yparxoyn plhrwmers tha dooume ti kanoume....!!! alla den ginete  ---- an ginei prepei na saga kai na gyrisw ta pada pisw           
                }

                double _dayOfExpiration = _dayOfExpirationService.SetDayOfExpiration(PaymentTypes.Free);


                var newPayment = new Payment(command.Id, command.CustomerId, command.Name, command.Email, command.PhoneNumber, command.Type, PaymentTypes.Free, true, _dayOfExpiration);
                await _paymentRepository.AddAsync(newPayment);

            }
            catch (Exception ex) {

                _logger.LogError($"Payment could not Created with error {ex.StackTrace}");
                await _busPublisher.PublishAsync(new CreatePaymentRejected(
                   command.CustomerId,
                   $"Payment with id: '{command.Id}' could not Created with error {ex.StackTrace}",
                   "Create Payment Exception"),
                   context);
                return;
            }

            return;
        }
    }
}

