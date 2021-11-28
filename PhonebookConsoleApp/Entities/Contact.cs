using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhonebookConsoleApp.Enums;

namespace PhonebookConsoleApp.Entities
{
    class Contact
    {
        public string nameAndSurname { get; set; }
        public string phoneNumber { get; set; }
        public ContactPreference Preference { get; set; }
    }
}