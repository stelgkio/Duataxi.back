using DuaTaxi.Service.Payments.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.Payments.Services.FindExistingPayments
{
    public interface IFindExistingPayments
    {
        Task FindPayment(string Id);

        Task<Payment> FindActivePayment(string CustomerId);
        Task<bool> CheckCustomerPayments(string CustomerId);

    }
}
