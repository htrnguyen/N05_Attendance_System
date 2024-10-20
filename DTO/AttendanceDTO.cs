using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AttendanceDTO
    {
        public int AttendanceID { get; set; }
        public string Status { get; set; }
        public string CheckInAt { get; set; }

        public string IpAddress { get; set; }
    }
}
