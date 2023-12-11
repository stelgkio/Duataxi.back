using DuaTaxi.Service.Payments.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.Payments.Repository
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);

        Task DeleteAsync(Payment payment);

        Task<Payment> GetAsync(string Id);

        Task<IEnumerable<Payment>> GetCustomerByIdAsync(string Id);
    }
}
