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
                        AddCall(new DateTime(2021, 11, 1, 10, 0, 0), CallStatus.Ended, 15),
                        AddCall(DateTime.Now, CallStatus.InProgress, 0)
                    }
                },
                {AddContact("Iva Ivic", "22222222222", ContactPreference.Normal), new List<Call>()
                    {
                        AddCall(new DateTime(2021, 9, 8, 6, 0, 0), CallStatus.Missed, 0),
                    }
                },
                {AddContact("Pero Peric", "33333333333", ContactPreference.Blocked), new List<Call>()
                    {
                        AddCall(new DateTime(2021, 11, 11, 0, 0, 0), CallStatus.Missed, 0),
                        AddCall(new DateTime(2021, 1, 1, 0, 0, 0), CallStatus.Missed, 0)
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
            ReturnToMenu(phonebook, "main");
        }

        static void AddNewContact(Dictionary<Contact, List<Call>> phonebook)
        {
            Console.WriteLine("+-------------------------------------+");
            Console.WriteLine("| Dodavanje novih kontakata u imenik: |");
            Console.WriteLine("+-------------------------------------+");

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
            var emptyCallsList = new List<Call>();
            phonebook.Add(AddContact(nameAndSurname, phoneNumber, Preference), emptyCallsList);

            ReturnToMenu(phonebook, "main");
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

                Console.Write("Unesite podatke za kontakt koji zelite obrisati:\n");
                var phoneNumberDelete = InputPhoneNumber();

                var keyFound = false;
                foreach (var record in phonebook)
                {
                    if (record.Key.phoneNumber == phoneNumberDelete)
                    {
                        keyFound = true;
                        Console.WriteLine($"\nZelite izbrisati kontakt: {record.Key.nameAndSurname}");
                        if (ConfirmAction())
                        {
                            if (record.Value.Any())
                                record.Value.Clear();
                            phonebook.Remove(record.Key);
                            Console.WriteLine("\nKontakt uspjesno izbrisan!");
                        }
                    }
                }
                if (!keyFound)
                    Console.WriteLine("\nNije pronaden kontakt s tim brojem mobitela!");
            }
            ReturnToMenu(phonebook, "main");
        }

        static Contact ContactExists(Dictionary<Contact, List<Call>> phonebook, string phoneNumber)
        {
            foreach (var record in phonebook)
            {
                if (record.Key.phoneNumber == phoneNumber)
                    return record.Key;
            }
            return null;
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

                Console.Write("Unesite podatke za kontakt kojem zelite editirati preferencu:\n");
                var phoneNumberEditPreference = InputPhoneNumber();

                var keyFound = false;
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
            ReturnToMenu(phonebook, "main");
        }

        static void PrintAllCallsByContact(Dictionary<Contact, List<Call>> phonebook)
        {
            Console.Clear();
            if (IsEmpty(phonebook))
            {
                Console.WriteLine("Nemate nijedan kontakt u imeniku!");
            }
            else
            {
                Console.WriteLine("+--------------------------------------+");
                Console.WriteLine("| Ispis svih poziva s nekim kontaktom: |");
                Console.WriteLine("+--------------------------------------+");
                Console.WriteLine();

                Console.Write("Unesite podatke za kontakt ciju povijest poziva zelite ispisati:\n");
                var phoneNumberEditPreference = InputPhoneNumber();
                
                var keyFound = false;
                var callsExist = false;
                var callsByContact = new List<Call>();
                foreach (var record in phonebook)
                {
                    if (record.Key.phoneNumber == phoneNumberEditPreference)
                    {
                        keyFound = true;
                        if (!record.Value.Any())
                        {
                            Console.WriteLine("\nNema zabiljezenih poziva s ovim kontaktom!");
                        }
                        else
                        {
                            callsExist = true;
                            Console.WriteLine($"\nSvi pozivi s kontaktom {record.Key.nameAndSurname} poredani vremenski od najnovijeg do najstarijeg:");
                            foreach (var call in record.Value)
                            {
                                callsByContact.Add(call);
                            }
                        }
                    }
                }
                if (callsExist)
                {
                    var sortedCallsByContact = from entry in callsByContact orderby entry.setupTime descending select entry;
                    foreach (var call in sortedCallsByContact)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Vrijeme uspostave poziva: {call.setupTime}");
                        Console.WriteLine($"Status poziva: {call.Status}");
                        Console.WriteLine($"Trajanje poziva: {call.duration} s");
                    }
                }
                if (!keyFound)
                    Console.WriteLine("\nNije pronaden kontakt s tim brojem mobitela!");
            }
            ReturnToMenu(phonebook, "sub");
        }

        static void MakeNewCall(Dictionary<Contact, List<Call>> phonebook)
        {
            Console.Clear();
            if (IsEmpty(phonebook))
            {
                Console.WriteLine("Nemate nijedan kontakt u imeniku!");
            }
            else
            {
                Console.WriteLine("+-------------------------+");
                Console.WriteLine("| Kreiranje novog poziva: |");
                Console.WriteLine("+-------------------------+");
                Console.WriteLine();

                Console.Write("Unesite podatke za kontakt koji zelite nazvati:\n");
                var phoneNumberMakeCall = InputPhoneNumber();

                var keyFound = false;
                foreach (var record in phonebook)
                {
                    if (record.Key.phoneNumber == phoneNumberMakeCall)
                    {
                        keyFound = true;
                        if (record.Key.Preference is ContactPreference.Blocked)
                        {
                            Console.WriteLine($"\nNemoguce uspostaviti poziv! Kontakt {record.Key.nameAndSurname} je blokiran!");
                        }
                        else if (AnyCallsInProgress(phonebook))
                        {
                            Console.WriteLine($"\nNemoguce uspostaviti poziv s {record.Key.nameAndSurname} dok je drugi poziv u tijeku!");
                            Console.WriteLine($"\nZelite li prekinuti poziv koji je u tijeku:");
                            if (ConfirmAction())
                            {
                                EndCallInProgress(phonebook, 20);
                                Console.WriteLine("\nPoziv uspjesno prekinut!");
                            }
                        }
                        else
                        {
                            Random random = new Random();
                            var choice = random.Next(1, 3);
                            switch ((CallStatus)choice)
                            {
                                case (CallStatus)1:
                                    record.Value.Add(AddCall(DateTime.Now, CallStatus.Missed, 0));
                                    Console.WriteLine($"\n{record.Key.nameAndSurname} je propustio Vas poziv.");
                                    break;
                                case (CallStatus)2:
                                    var randomDuration = random.Next(1, 21);
                                    record.Value.Add(AddCall(DateTime.Now, CallStatus.InProgress, 0));
                                    Console.WriteLine($"\n{record.Key.nameAndSurname} je odgovorio na Vas poziv.");
                                    EndCallInProgress(phonebook, randomDuration);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                if (!keyFound)
                    Console.WriteLine("\nNemoguce uspostaviti poziv! Nije pronaden kontakt s tim brojem mobitela!");
            }
            ReturnToMenu(phonebook, "sub");
        }

        static bool AnyCallsInProgress(Dictionary<Contact, List<Call>> phonebook)
        {
            foreach (var record in phonebook)
            {
                if (record.Value.Any())
                {
                    foreach (var call in record.Value)
                    {
                        if (call.Status == CallStatus.InProgress)
                            return true;
                    }
                }
            }
            return false;
        }

        static void EndCallInProgress(Dictionary<Contact, List<Call>> phonebook, int callDuration)
        {
            foreach (var record in phonebook)
            {
                if (record.Value.Any())
                {
                    foreach (var call in record.Value)
                    {
                        if (call.Status == CallStatus.InProgress)
                        {
                            call.Status = CallStatus.Ended;
                            call.duration = callDuration;
                        }
                    }
                }
            }
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

        static void ReturnToMenu(Dictionary<Contact, List<Call>> phonebook, string type)
        {
            Console.WriteLine();
            if (type is "main")
            {
                Console.WriteLine("+---------------------------------+");
                Console.WriteLine("| 0 - Povratak na glavni izbornik |");
                Console.WriteLine("+---------------------------------+");
            }
            else if (type is "sub")
            {
                Console.WriteLine("+-------------------------+");
                Console.WriteLine("| 0 - Povratak na submenu |");
                Console.WriteLine("+-------------------------+");
            }

            int userReturnChoice;
            bool tryParseSuccess;
            do
            {
                Console.WriteLine("\nUnesite svoj odabir:");
                tryParseSuccess = int.TryParse(Console.ReadLine().Trim(), out userReturnChoice);
                if (!tryParseSuccess || userReturnChoice != 0)
                    Console.WriteLine("Neispravan unos!");
            } while (!tryParseSuccess || userReturnChoice != 0);

            if (type is "main")
                MainMenu(phonebook);
            else if (type is "sub")
                ManageContactSubmenu(phonebook);
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
                        PrintAllCallsByContact(phonebook);
                        break;
                    case 2:
                        MakeNewCall(phonebook);
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