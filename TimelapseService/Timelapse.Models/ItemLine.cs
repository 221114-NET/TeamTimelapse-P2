using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timelapse.Models
{
    public class ItemLine
    {
        public Guid OrderId { get; set; }
        public Guid WatchId { get; set; }
        public Watch Model { get; set; }
        public int Quantity { get; set; }

        public ItemLine() {}
        public ItemLine(Guid orderId, Guid watchId, int quantity)
        {
            OrderId = orderId;
            WatchId = watchId;
            Quantity = quantity;
        }
    }
}