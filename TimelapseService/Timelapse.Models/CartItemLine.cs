using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timelapse.Models
{
    public class CartItemLine
    {
        public Guid Id { get; set; }
        public Watch Model { get; set; }
        public int Quantity { get; set; }

        public CartItemLine() {}
        public CartItemLine(Watch model, int quantity)
        {
            Model = model;
            Quantity = quantity;
        }
    }
}