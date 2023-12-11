using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.CoreApi.Models.TaxiApi
{

    public class TaxiDriverStatus
    {

        public string CustomerId { get; protected set; }
        public string ConnectionId { get; protected set; }

        public bool Status { get; protected set; }


        public TaxiDriverStatus(string customerId, string connectionId, bool status)
        {
            CustomerId = customerId;
            ConnectionId = connectionId;
            Status = status;

        }


    }
}
