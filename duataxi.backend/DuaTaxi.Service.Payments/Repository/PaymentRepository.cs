using DuaTaxi.Common.Mongo;
using DuaTaxi.Service.Payments.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.Payments.Repository
{
    public class PaymentRepository   : IPaymentRepository
    {
        IMongoRepository<Payment> _repository;
        public PaymentRepository(IMongoRepository<Payment> repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(Payment payment)
            => await _repository.AddAsync(payment);

        public async Task DeleteAsync(Payment payment)
            => await _repository.DeleteAsync(payment.Id);

        public async Task<Payment> GetAsync(string Id)
            => await _repository.GetAsync(Id);

        public async Task<IEnumerable<Payment>> GetCustomerByIdAsync(string Id)
            => await _repository.FindAsync(c => c.CustomerId == Id);

        public async Task UpdateAsync(Payment payment)
            => await _repository.UpdateAsync(payment);
    }
}
