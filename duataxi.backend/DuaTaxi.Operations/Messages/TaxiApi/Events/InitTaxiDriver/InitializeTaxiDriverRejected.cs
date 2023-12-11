﻿using DuaTaxi.Common.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Operations.Messages.TaxiApi.Events
{
    [MessageNamespace("taxiapi")]
    public class InitializeTaxiDriverRejected : IRejectedEvent
    {
        public string CustomerId { get; protected set; }
        public string Reason { get; protected set; }
        public string Code { get; protected set; }

        [JsonConstructor]
        public InitializeTaxiDriverRejected(string customerId, string reason, string code)
        {
            CustomerId = customerId;
            this.Reason = reason;
            this.Code = code;
        }

    }
}
