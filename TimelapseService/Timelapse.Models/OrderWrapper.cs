using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timelapse.Models
{
    public class OrderWrapper
    {
        public User User { get; set; }
        public List<ItemLine> Watches { get; set; }

        public OrderWrapper() {}
        public OrderWrapper(User user, List<ItemLine> watches)
        {
            User = user;
            Watches = watches;
        }
    }
}