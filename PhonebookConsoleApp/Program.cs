using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PhonebookConsoleApp.Entities;
using PhonebookConsoleApp.Enums;

namespace PhonebookConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var phonebook = LoadDefaultPhonebook();
            
            MainMenu(phonebook);
        }

        static Dictionary<Contact, List<Call>> LoadDefaultPhonebook()
        {
            return new Dictionary<Contact, List<Call>>
            {
                {AddContact("Ante Antic", "11111111111", ContactPreference.Favorite), new List<Call>()
                    {
                        AddCall(new DateTime(2021, 11, 1, 10, 0, 0), CallStatus.Missed, 11),
                        AddCall(DateTime.Now, CallStatus.Ended, 20)
                    }
                },
                {AddContact("Iva Ivic", "22222222222", ContactPreference.Normal), new List<Call>()
                    {
                        AddCall(new DateTime(2021, 9, 8, 6, 0, 0), CallStatus.Missed, 11),
                        AddCall(DateTime.Now, CallStatus.Ended, 20)
                    }
                },
                {AddContact("Pero Peric", "33333333333", ContactPreference.Blocked), new List<Call>()
                    {
                        AddCall(new DateTime(2021, 11, 11, 0, 0, 0), CallStatus.Missed, 8),
                        AddCall(new DateTime(2021, 1, 1, 0, 0, 0), CallStatus.Missed, 13)
                    }
                }
            };
        }

        static Contact AddContact(string nameAndSurname, string phoneNumber, ContactPreference Preference)
        {
            var addedContact = new Contact();
            addedContact.AddValue(nameAndSurname, phoneNumber, Preference);
            return addedContact;
        }

        static Call AddCall(DateTime setupTime, CallStatus Status, int duration)
        {
            var addedCall = new Call();
            addedCall.AddValue(setupTime, Status, duration);
            return addedCall;
        }

        static bool IsEmpty(Dictionary<Contact, List<Call>> phonebook)
        {
            return phonebook.Count is 0 ? true : false;
        }

        static void PrintAllContacts(Dictionary<Contact, List<Call>> phonebook)
        {
            if (IsEmpty(phonebook))
            {
                Console.WriteLine("Nemate nijedan kontakt u imeniku!");
            }
            else
            {
                Console.WriteLine("+-----------------------+");
                Console.WriteLine("| Ispis svih kontakata: |");
                Console.WriteLine("+-----------------------+");

                foreach (var record in phonebook)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Ime i prezime: {record.Key.nameAndSurname}");
                    Console.WriteLine($"Broj mobitela: {record.Key.phoneNumber}");
                    Console.WriteLine($"Preferenca: {record.Key.Preference}");
                }
            }
            ReturnToMainMenu(phonebook);
        }

        static void AddNewContact(Dictionary<Contact, List<Call>> phonebook)
        {
            Console.WriteLine("+-------------------------------------+");
            Console.WriteLine("| Dodavanje novih kontakata u imenik: |");
            Console.WriteLine("+-------------------------------------+");
            Console.WriteLine();

            var nameAndSurname = InputNameAndSurname();
            var phoneNumber = "";
            do
            {
                if (PhoneNumberDuplicate(phoneNumber, phonebook))
                {
                    Console.WriteLine("Vec postoji kontakt s tim brojem mobitela u imeniku!");
                }
                phoneNumber = InputPhoneNumber();
            } while (PhoneNumberDuplicate(phoneNumber, phonebook));
            var Preference = (ContactPreference)InputContactPreference();
            phonebook.Add(AddContact(nameAndSurname, phoneNumber, Preference), null);

            ReturnToMainMenu(phonebook);
        }

        static string InputNameAndSurname()
        {
            string name, surname;
            Console.WriteLine();
            do
            {
                Console.WriteLine("Unesite ime kontakta:");
                name = Console.ReadLine().Trim();
                if (!ValidNameAndSurname(name))
                    Console.WriteLine("Neispravan unos!");
            } while (!ValidNameAndSurname(name));
            Console.WriteLine();
            do
            {
                Console.WriteLine("Unesite prezime kontakta:");
                surname = Console.ReadLine().Trim();
                if (!ValidNameAndSurname(surname))
                    Console.WriteLine("Neispravan unos!");
            } while (!ValidNameAndSurname(surname));
            return name + " " + surname;
        }

        static bool ValidNameAndSurname(string nameAndSurname)
        {
            if (nameAndSurname.Length >= 1 && Regex.Match(nameAndSurname, "^[A-Z][a-zA-Z]*$").Success)
                return true;
            return false;
        }

        static string InputPhoneNumber()
        {
            string phoneNumber;
            Console.WriteLine();
            do
            {
                Console.WriteLine("Unesite broj mobitela kontakta:");
                phoneNumber = Console.ReadLine().Trim();
                if (!ValidPhoneNumber(phoneNumber))
                    Console.WriteLine("Neispravan broj mobitela!");
            } while (!ValidPhoneNumber(phoneNumber));
            return phoneNumber;
        }

        static bool ValidPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.All(char.IsDigit) && phoneNumber.Length <= 14)
                return true;
            return false;
        }

        static int InputContactPreference()
        {
            int userPreferenceChoice;
            bool tryParseSuccess;
            Console.WriteLine();
            Console.WriteLine("+-- Preference za kontakt --+");
            Console.WriteLine("| 0 - Favorit               |");
            Console.WriteLine("| 1 - Normalno              |");
            Console.WriteLine("| 2 - Blokiran              |");
            Console.WriteLine("+---------------------------+");
            do
            {
                Console.WriteLine("Unesite preferencu za kontakt:");
                tryParseSuccess = int.TryParse(Console.ReadLine().Trim(), out userPreferenceChoice);
                if (!tryParseSuccess || userPreferenceChoice < 0 || userPreferenceChoice > 2)
                    Console.WriteLine("Neispravan unos!");
            } while (!tryParseSuccess || userPreferenceChoice < 0 || userPreferenceChoice > 2);
            return userPreferenceChoice;
        }

        static bool PhoneNumberDuplicate(string number, Dictionary<Contact, List<Call>> phonebook)
        {
            if (IsEmpty(phonebook))
                return false;
            else
            {
                foreach (var record in phonebook)
                {
                    if (record.Key.phoneNumber == number)
                        return true;
                }
                return false;
            }
        }

        static void DeleteContact(Dictionary<Contact, List<Call>> phonebook)
        {
            if (IsEmpty(phonebook))
            {
                Console.WriteLine("Nemate nijedan kontakt u imeniku!");
            }
            else
            {
                Console.WriteLine("+--------------------------------+");
                Console.WriteLine("| Brisanje kontakata iz imenika: |");
                Console.WriteLine("+--------------------------------+");
                Console.WriteLine();

                var keyFound = false;
                Console.Write("Unesite podatke za kontakt koji zelite obrisati:\n");
                var phoneNumberDelete = InputPhoneNumber();
                foreach (var record in phonebook)
                {
                    if (record.Key.phoneNumber == phoneNumberDelete)
                    {
                        keyFound = true;
                        Console.WriteLine($"\nZelite izbrisati kontakt: {record.Key.nameAndSurname}");
                        if(ConfirmAction())
                        {
                            if (!(record.Value is null))
                                record.Value.Clear();
                            phonebook.Remove(record.Key);
                            Console.WriteLine("\nKontakt uspjesno izbrisan!");
                        }
                    }
                }
                if (!keyFound)
                    Console.WriteLine("\nNije pronaden kontakt s tim brojem mobitela!");
            }
            ReturnToMainMenu(phonebook);
        }

        static bool ConfirmAction()
        {
            int userDecision;
            bool tryParseSuccess;
            Console.WriteLine("+--- ! Potvrdite svoju odluku ---+");
            Console.WriteLine("| 1 - Potvrda                    |");
            Console.WriteLine("| 2 - Odustani                   |");
            Console.WriteLine("+--------------------------------+");
            do
            {
                Console.WriteLine("\nUnesite svoj odabir:");
                tryParseSuccess = int.TryParse(Console.ReadLine().Trim(), out userDecision);
                if (!tryParseSuccess || userDecision < 1 || userDecision > 2)
                    Console.WriteLine("Neispravan unos!");
            } while (!tryParseSuccess || userDecision < 1 || userDecision > 2);
            if (userDecision == 1)
                return true;
            return false;
        }

        static void EditContactPreference(Dictionary<Contact, List<Call>> phonebook)
        {
            if (IsEmpty(phonebook))
            {
                Console.WriteLine("Nemate nijedan kontakt u imeniku!");
            }
            else
            {
                Console.WriteLine("+---------------------------------+");
                Console.WriteLine("| Editiranje preference kontakta: |");
                Console.WriteLine("+---------------------------------+");
                Console.WriteLine();

                var keyFound = false;
                Console.Write("Unesite podatke za kontakt kojem zelite editirati preferencu:\n");
                var phoneNumberEditPreference = InputPhoneNumber();
                foreach (var record in phonebook)
                {
                    if (record.Key.phoneNumber == phoneNumberEditPreference)
                    {
                        keyFound = true;
                        Console.WriteLine($"\nEditirate preferencu za kontakt: {record.Key.nameAndSurname}");
                        if (ConfirmAction())
                        {
                            var userPreferenceChoiceEdit = InputContactPreference();
                            record.Key.Preference = (ContactPreference)userPreferenceChoiceEdit;
                            Console.WriteLine("\nKontaktu uspjesno editirana preferenca!");
                        }
                    }
                }
                if (!keyFound)
                    Console.WriteLine("\nNije pronaden kontakt s tim brojem mobitela!");
            }
            ReturnToMainMenu(phonebook);
        }

            static void PrintMainMenu()
        {
            Console.Clear();
            Console.WriteLine("+----------------- MENU -----------------+");
            Console.WriteLine("| 1 - Ispis svih kontakata               |");
            Console.WriteLine("| 2 - Dodavanje novih kontakata u imenik |");
            Console.WriteLine("| 3 - Brisanje kontakata iz imenika      |");
            Console.WriteLine("| 4 - Editiranje preference kontakta     |");
            Console.WriteLine("| 5 - Upravljanje kontaktom              |");
            Console.WriteLine("| 6 - Ispis svih poziva                  |");
            Console.WriteLine("| 7 - Izlaz iz aplikacije                |");
            Console.WriteLine("+----------------------------------------+");
        }

        static void MainMenu(Dictionary<Contact, List<Call>> phonebook)
        {
            PrintMainMenu();

            int userMenuChoice;
            bool tryParseSuccess;
            do
            {
                Console.WriteLine("\nUnesite svoj odabir:");
                tryParseSuccess = int.TryParse(Console.ReadLine().Trim(), out userMenuChoice);
                
                switch (userMenuChoice)
                {
                    case 1:
                        Console.Clear();
                        PrintAllContacts(phonebook);
                        break;
                    case 2:
                        Console.Clear();
                        AddNewContact(phonebook);
                        break;
                    case 3:
                        Console.Clear();
                        DeleteContact(phonebook);
                        break;
                    case 4:
                        Console.Clear();
                        EditContactPreference(phonebook);
                        break;
                    case 5:
                        Console.Clear();
                        ManageContactSubmenu(phonebook);
                        break;
                    case 6:
                        Console.Clear();
                        // PrintAllCalls(phonebook);
                        break;
                    case 7:
                        Console.Clear();
                        Console.WriteLine("+-------------------------------+");
                        Console.WriteLine("| Izlaz iz aplikacije. Pozdrav! |");
                        Console.WriteLine("+-------------------------------+");
                        break;
                    default:
                        Console.WriteLine("Neispravan unos!");
                        break;
                }

            } while (!tryParseSuccess || userMenuChoice < 1 || userMenuChoice > 7);
        }

        static void ReturnToMainMenu(Dictionary<Contact, List<Call>> phonebook)
        {
            Console.WriteLine();
            Console.WriteLine("+---------------------------------+");
            Console.WriteLine("| 0 - Povratak na glavni izbornik |");
            Console.WriteLine("+---------------------------------+");
            int userReturnChoice;
            bool tryParseSuccess;
            do
            {
                Console.WriteLine("\nUnesite svoj odabir:");
                tryParseSuccess = int.TryParse(Console.ReadLine().Trim(), out userReturnChoice);
                if (!tryParseSuccess || userReturnChoice != 0)
                    Console.WriteLine("Neispravan unos!");
            } while (!tryParseSuccess || userReturnChoice != 0);
            MainMenu(phonebook);
        }

        static void PrintManageContactSubmenu()
        {
            Console.Clear();
            Console.WriteLine("+--------------------- SUBMENU: Upravljanje kontaktom ---------------------+");
            Console.WriteLine("| 1 - Ispis svih poziva sa tim kontaktom (od najnovijeg prema najstarijem) |");
            Console.WriteLine("| 2 - Kreiranje novog poziva                                               |");
            Console.WriteLine("| 3 - Izlaz iz podmenua                                                    |");
            Console.WriteLine("+--------------------------------------------------------------------------+");
        }

        static void ManageContactSubmenu(Dictionary<Contact, List<Call>> phonebook)
        {
            PrintManageContactSubmenu();

            int userSubmenuChoice;
            bool tryParseSuccess;
            do
            {
                Console.WriteLine("\nUnesite svoj odabir:");
                tryParseSuccess = int.TryParse(Console.ReadLine().Trim(), out userSubmenuChoice);
                
                switch (userSubmenuChoice)
                {
                    case 1:
                        // PrintAllCallsByContact(phonebook);
                        break;
                    case 2:
                        // MakeNewCall(phonebook);
                        break;
                    case 3:
                        MainMenu(phonebook);
                        break;
                    default:
                        Console.WriteLine("Neispravan unos!");
                        break;
                }

            } while (userSubmenuChoice < 1 || userSubmenuChoice > 3);
        }
    }
}