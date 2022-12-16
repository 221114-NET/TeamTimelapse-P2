using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timelapse.Models
{
    public class Cart
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid WatchId { get; set; }

        public Cart() {}
        public Cart(Guid orderId, Guid customerId, Guid watchId)
        {
            OrderId = orderId;
            CustomerId = customerId;
            WatchId = watchId;
        }
    }
}