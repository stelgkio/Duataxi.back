using DuaTaxi.Service.Payments.Entities.Models;
using DuaTaxi.Service.Payments.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.Payments.Services.FindExistingPayments
{
    public class FindExitingPayments  : IFindExistingPayments
    {
        private readonly IPaymentRepository _paymentRepository;

        public FindExitingPayments(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async  Task<bool> CheckCustomerPayments(string CustomerId)
        {

            var customerPayments = await _paymentRepository.GetCustomerByIdAsync(CustomerId);

            if (customerPayments.Count() == 0)
            {
                return false;
            }         

            return true;
        }

        public async Task<Payment> FindActivePayment(string CustomerId)
        {
            var customerPayments = await _paymentRepository.GetCustomerByIdAsync(CustomerId);            

            var activePayment=   customerPayments.Where(x => x.Active).FirstOrDefault();

            return activePayment;
        }

       
        public Task FindPayment(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
