using DuaTaxi.Common.Types;
using DuaTaxi.Service.Payments.Entities.Models;
using DuaTaxi.Service.Payments.Repository;
using DuaTaxi.Service.Payments.Services.FindExistingPayments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.Payments.Services.DayOfExpiration
{
    public class DayOfExpirationService : IDayOfExpirationService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IFindExistingPayments _findExistingPayments;

        public DayOfExpirationService(IPaymentRepository paymentRepository, IFindExistingPayments findExistingPayments)
        {
            _paymentRepository = paymentRepository;
            _findExistingPayments = findExistingPayments;
        }

        public async Task<double> GetLeftDaysOfExpirationByCustomerId(string CustomerId = null)
        {
            if (CustomerId is null)
                return 0;

            var payment = await _findExistingPayments.FindActivePayment(CustomerId);

            return CalculateLeftDays(payment.CreatedDate, payment.DayOfExpiration);
        }

        public async Task<double> GetLeftDaysOfExpirationById(string PaymentId = null)
        {
            if (PaymentId is null)
                return 0;

            var payment = await _paymentRepository.GetAsync(PaymentId);

            return CalculateLeftDays(payment.CreatedDate, payment.DayOfExpiration);
        }

        public double SetDayOfExpiration(PaymentTypes types)
        {
            switch (types)
            {
                case PaymentTypes.Free:
                    return 60;                    
                case PaymentTypes.PayPal:
                    return 30;                    
                case PaymentTypes.Iban:
                    return 30;                    
                case PaymentTypes.Card:
                    return 30;                    
                default:
                    return 0;

            }


        }


        private double CalculateLeftDays(DateTime createdDay, double DayOfExpiration)
        {
            double x = (createdDay.AddDays(DayOfExpiration) - DateTime.UtcNow).Days;
            return x;                                                               
        }
    }
}
