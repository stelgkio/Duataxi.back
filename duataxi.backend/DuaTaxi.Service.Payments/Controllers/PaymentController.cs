using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuaTaxi.Service.Payments.Entities.Models;
using DuaTaxi.Service.Payments.Services.DayOfExpiration;
using DuaTaxi.Service.Payments.Services.FindExistingPayments;
using Microsoft.AspNetCore.Mvc;

namespace DuaTaxi.Service.Payments.Controllers
{
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IFindExistingPayments _findExistingPayments;
        private readonly IDayOfExpirationService _dayOfExpirationService;
        public PaymentController(IFindExistingPayments findExistingPayments , IDayOfExpirationService dayOfExpirationService)
        {
            _findExistingPayments = findExistingPayments;
            _dayOfExpirationService = dayOfExpirationService;
        }
        [HttpGet]
        [Route("/{customerId}")]
        public async Task<Payment> GetByCustomerId(string customerId)
        {
            var activepayment =await _findExistingPayments.FindActivePayment(customerId);
            activepayment.DayOfExpiration = await _dayOfExpirationService.GetLeftDaysOfExpirationById(activepayment.Id);
            return activepayment;
        }
    }
}