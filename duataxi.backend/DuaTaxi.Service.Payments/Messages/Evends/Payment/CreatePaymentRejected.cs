using DuaTaxi.Common.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.Payments.Messages.Evends.Payment
{
    public class CreatePaymentRejected : IRejectedEvent
    {
        public Guid CustomerId { get; protected set; }
        public string Reason { get; protected set; }
        public string Code { get; protected set; }

        [JsonConstructor]
        public CreatePaymentRejected(string customerId, string reason, string code)
        {
            CustomerId = Guid.Parse(customerId);
            this.Reason = reason;
            this.Code = code;
        }

    }
}
