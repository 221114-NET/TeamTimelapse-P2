using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timelapse.Models
{
    public class CartWrapper
    {
        public User User { get; set; }
        public List<CartItemLine> Watches { get; set; }

        public CartWrapper() {}
        public CartWrapper(User user, List<CartItemLine> watches)
        {
            User = user;
            Watches = watches;
        }
    }
}