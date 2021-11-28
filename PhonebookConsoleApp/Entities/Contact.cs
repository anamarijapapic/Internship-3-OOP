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
        public string _nameAndSurname;
        public string _phoneNumber;
        public ContactPreference _Preference = ContactPreference.Normal;

        public string nameAndSurname { get => _nameAndSurname; set => _nameAndSurname = value; }
        public string phoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
        public ContactPreference Preference { get => _Preference; set => _Preference = value; }

        public Contact AddValue(string nameAndSurname, string phoneNumber, ContactPreference Preference)
        {
            _nameAndSurname = nameAndSurname;
            _phoneNumber = phoneNumber;
            _Preference = Preference;
            return this;
        }
    }
}