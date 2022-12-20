using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timelapse.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public List<ItemLine> Watches { get; set; }
        
        

        public Order() {}
        public Order(Guid orderId, Guid customerId, List<ItemLine> watches)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Watches = watches;
        }
    }
}