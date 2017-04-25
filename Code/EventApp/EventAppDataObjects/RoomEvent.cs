using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAppDataObjects
{
    public class RoomEvent
    {
        [Display(Name="Event ID")]
        public int EventID { get; set; }

        public string Name { get; set; }

        public string Date { get; set; }

        [Display(Name="Individual Price")]
        public decimal IndividualPrice { get; set; }

        [Display(Name="Tickets Reserved")]
        public int TicketsReserved { get; set; }

        [Display(Name="Total Price")]
        public decimal TotalPrice { get; set; }
    }
}
