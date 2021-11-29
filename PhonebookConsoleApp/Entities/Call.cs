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
        private DateTime _setupTime;
        private CallStatus _Status;
        private int _duration;

        public DateTime setupTime { get => _setupTime; set => _setupTime = value; }
        public CallStatus Status { get => _Status; set => _Status = value; }
        public int duration { get => _duration; set => _duration = value; }

        public Call AddValue(DateTime setupTime, CallStatus Status, int duration)
        {
            _setupTime = setupTime;
            _duration = duration;
            _Status = Status;
            return this;
        }
    }
}