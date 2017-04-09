using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAppDataObjects
{
    public class EventWithEmployee : Event
    {
        public int ID
        {
            get
            {
                return EventID;
            }
            set
            {
                EventID = value;
            }
        }

        public Employee EmployeeCreater { get; set; }
    }
}
