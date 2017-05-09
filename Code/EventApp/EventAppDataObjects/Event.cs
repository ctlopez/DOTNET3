using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EventAppDataObjects
{
    public class Event
    {
        public int EventID { get; set; }

        [Required(ErrorMessage="You must supply a name.")]
        [StringLength(100, ErrorMessage="Cannot exceed 100 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You must supply a description.")]
        [StringLength(300, ErrorMessage = "Cannot exceed 300 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "You must supply a date.")]
        [FutureDate(ErrorMessage="The date must be in the future.")]
        public string Date { get; set; }
        [Required(ErrorMessage = "You must supply a time.")]
        [RegularExpression("^((1[0-2]|[1-9]):([0-5][0-9]) ([AP][M]))", ErrorMessage="Time must be H:MM [AM/PM]")]
        public string Time { get; set; }
        [Required(ErrorMessage = "You must supply a location.")]
        [StringLength(300, ErrorMessage = "Cannot exceed 300 characters.")]
        public string Location { get; set; }
        [Required(ErrorMessage = "You must supply the maximum number of seats.")]
        [Display(Name="Max Seats")]
        public int MaxSeats { get; set; }
        [Required(ErrorMessage="You must supply a valid price.")]
        public decimal Price { get; set; }
        [Display(Name="Added By")]
        public int AddedBy { get; set; }

        public bool Active { get; set; }
    }
}
