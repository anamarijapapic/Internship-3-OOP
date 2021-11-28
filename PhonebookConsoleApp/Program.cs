using System;
using System.Collections.Generic;
using PhonebookConsoleApp.Entities;

namespace PhonebookConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var phonebook = new Dictionary<Contact, List<Call>>();
            
            MainMenu(phonebook);
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
                        // PrintAllContacts(phonebook);
                        break;
                    case 2:
                        Console.Clear();
                        // AddNewContact(phonebook);
                        break;
                    case 3:
                        Console.Clear();
                        // DeleteContact(phonebook);
                        break;
                    case 4:
                        Console.Clear();
                        // EditContactPreference(phonebook);
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
                        Console.WriteLine("Izlaz iz aplikacije. Pozdrav!");
                        break;
                    default:
                        Console.WriteLine("Neispravan unos!");
                        break;
                }

            } while (!tryParseSuccess || userMenuChoice < 1 || userMenuChoice > 7);
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