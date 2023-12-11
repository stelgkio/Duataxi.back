using System;
using DuaTaxi.Common.Messages;
using Newtonsoft.Json;

namespace DuaTaxi.Services.Operations.Messages.Customers.Events
{
    [MessageNamespace("customers")]
    public class ProductAddedToCart : IEvent
    {
        public Guid CustomerId { get; }
        public Guid ProductId { get; }
        public int Quantity { get; }

        [JsonConstructor]
        public ProductAddedToCart(Guid customerId, Guid productId,
            int quantity)
        {
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
        }        
    }
}