using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAppDataObjects
{
    public class User : Employee
    {
        public List<Role> Roles { get; set; }
    }
}
