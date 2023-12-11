using System;
using DuaTaxi.Common.Messages;
using Newtonsoft.Json;

namespace DuaTaxi.Services.Operations.Messages.Customers.Events
{
    [MessageNamespace("customers")]
    public class AddProductToCartRejected : IRejectedEvent
    {
        public Guid CustomerId { get; }
        public Guid ProductId { get; }
        public int Quantity { get; }
        public string Reason { get; }
        public string Code { get; }

        [JsonConstructor]
        public AddProductToCartRejected(Guid customerId, Guid productId,
            int quantity, string reason, string code)
        {
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
            Reason = reason;
            Code = code;
        }
    }
}