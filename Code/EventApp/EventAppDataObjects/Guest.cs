using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EventAppDataObjects
{
    public class Guest
    {
        //DTO
        public int GuestID { get; set; }
        
        [Display(Name="Room Number")]
        public string RoomID { get; set; }

        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Display(Name="Last Name")]
        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }
    }
}
