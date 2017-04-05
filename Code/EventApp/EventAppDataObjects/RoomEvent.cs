using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAppDataObjects
{
    public class RoomEvent
    {
        public int EventID { get; set; }

        public string Name { get; set; }

        public string Date { get; set; }

        public decimal IndividualPrice { get; set; }

        public int TicketsReserved { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
