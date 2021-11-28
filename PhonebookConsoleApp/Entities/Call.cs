using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhonebookConsoleApp.Enums;

namespace PhonebookConsoleApp.Entities
{
    class Call
    {
        public DateTime setupTime { get; set; }
        public CallStatus Status { get; set; }
    }
}