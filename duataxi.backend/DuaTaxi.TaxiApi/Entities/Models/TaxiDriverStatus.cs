using DuaTaxi.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuaTaxi.Common.Messages;

namespace DuaTaxi.Service.TaxiApi.Entities
{
    public class TaxiDriverStatus : BaseEntity
    {

        public string CustomerId { get;  protected set; }
        public string ConnectionId { get; protected set; }  
        public bool Status { get; protected set; }

        public double? DayOfExpiration { get; protected set; }




        public TaxiDriverStatus( string customerId, string connectionId, bool status ,string error, double? DayOfExpiration=null)
        {   
            CustomerId = customerId;
            ConnectionId = connectionId;
            Status = status;
            Error = error;
            this.DayOfExpiration = DayOfExpiration;


        }

     
    }
}
