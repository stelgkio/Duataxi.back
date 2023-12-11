using System;
using DuaTaxi.Common.Messages;
using Newtonsoft.Json;

namespace DuaTaxi.Services.Operations.Messages.Customers.Events
{
    [MessageNamespace("customers")]
    public class CreateCustomerRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public string Reason { get; }
        public string Code { get; }

        [JsonConstructor]
        public CreateCustomerRejected(Guid id, string reason, string code)
        {
            Id = id;
            Reason = reason;
            Code = code;
        }
    }
}