using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using EventAppDataObjects;

namespace EventAppMVCPresentationLayer.Models
{
    public class PurchaseModel
    {
        [Key]
        public int EventID { get; set; }

        [Required(ErrorMessage = "You must supply a name.")]
        [StringLength(100, ErrorMessage = "Cannot exceed 100 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You must supply a description.")]
        [StringLength(300, ErrorMessage = "Cannot exceed 300 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "You must supply a date.")]
        public string Date { get; set; }
        [Required(ErrorMessage = "You must supply a time.")]
        public string Time { get; set; }
        [Required(ErrorMessage = "You must supply a location.")]
        [StringLength(300, ErrorMessage = "Cannot exceed 300 characters.")]
        public string Location { get; set; }
        [Required(ErrorMessage = "You must supply the maximum number of seats.")]
        [Display(Name = "Max Seats")]
        public int MaxSeats { get; set; }
        [Required(ErrorMessage = "You must supply a valid price.")]
        public decimal Price { get; set; }
        [Display(Name = "Added By")]
        public int AddedBy { get; set; }

        public bool Active { get; set; }

        public Employee EmployeeCreater { get; set; }

        [Display(Name = "Tickets Left")]
        public int CurrentAmount { get; set; }

        [Required(ErrorMessage="You must supply a number to purchase")]
        [Range(1, int.MaxValue, ErrorMessage="Please enter a valid positive integer")]
        [LessThan("CurrentAmount")]
        public int Quantity { set; get; }

        public string RoomId { set; get; }
    }
}