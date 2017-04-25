using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventAppMVCPresentationLayer.Models
{
    /// <summary>
    /// Christian Lopez
    /// 2017/04/25
    /// </summary>
    public class EventsForAccountViewModel
    {
        [Key]
        public string RoomNumber { get; set; }

        public decimal TotalAmount { get; set; }

        public List<EventAppDataObjects.RoomEvent> EventsWithRoom { get; set; }
    }
}