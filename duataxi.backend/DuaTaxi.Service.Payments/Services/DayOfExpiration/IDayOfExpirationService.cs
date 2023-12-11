using DuaTaxi.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.Payments.Services.DayOfExpiration
{
    public interface IDayOfExpirationService
    {
        double SetDayOfExpiration(PaymentTypes types);

        Task<double> GetLeftDaysOfExpirationByCustomerId(string CustomerId = null);

        Task<double> GetLeftDaysOfExpirationById(string PaymentId = null);
    }
}
