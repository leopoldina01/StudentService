using System;
using System.Collections.Generic;
using System.Text;
using Projekat.Manager;
using Projekat.Model;

namespace Projekat.Console
{
    class AdresaConsoleView : ConsoleView
    {
        private AdresaManager adresaManager;

        public AdresaConsoleView(AdresaManager adresaManager)
        {
            this.adresaManager = adresaManager;
        }

        public void PrintAdrese(List<Adresa> adrese)
        {
            System.Console.WriteLine("Adrese: ");
            foreach (Adresa adresa in adrese)
            {
                System.Console.WriteLine(adresa);
            }
        }

        public Adresa InputAdresa()
        {
            Adresa adresa = new Adresa();

            System.Console.Write("Unesi ulicu: ");
            adresa.Ulica = System.Console.ReadLine();

            //dodati proveru za uneti broj
            while (true)
            {
                System.Console.Write("Unesi broj (broj ili broj i slovo): ");
                string broj = System.Console.ReadLine();

                if (broj.Length > 4)
                {
                    System.Console.WriteLine("Format ulicnog broja nije ispravan. Ponovi unos");
                    continue;
                }

                int i = 0;
                bool dobarUnos = true;
                while (i < broj.Length - 1)
                {
                    if (i == 0 && (broj[i] < '0' || broj[i] > '9'))
                    {
                        System.Console.WriteLine("Format broja nije ispravan. Broj ulice mora poceti cifrom. Ponovi unos");
                        dobarUnos = false;
                        break;
                    }

                    if (broj[i] < '0' || broj[i] > '9')
                    {
                        System.Console.WriteLine("Format broja nije ispravan. Ponovi unos");
                        dobarUnos = false;
                        break;
                    }

                    i++;
                }

                if (i == (broj.Length - 1) && dobarUnos)
                {
                    if ((broj[i] < '0' || broj[i] > '9') && !char.IsLetter(broj[i]))
                    {
                        System.Console.WriteLine("Format broja nije ispravan. Broj ulice mora poceti cifrom. Ponovi unos");
                        dobarUnos = false;
                        continue;
                    }
                    i++;
                }

                if (i == broj.Length && dobarUnos)
                {
                    adresa.Broj = broj;
                    break;
                }
            }


            System.Console.Write("Unesi grad: ");
            adresa.Grad = System.Console.ReadLine();

            System.Console.Write("Unesi drzavu: ");
            adresa.Drzava = System.Console.ReadLine();

            return adresa;
        }

        private int InputId()
        {
            System.Console.WriteLine("Unesi id adrese: ");
            return SafeInputInt();
        }

        private void UpdateAdresa()
        {
            int id = InputId();
            Adresa adresa = InputAdresa();
            adresa.Id = id;
            Adresa updatedAdresa = adresaManager.updateAdresa(adresa);
            if (updatedAdresa == null)
            {
                System.Console.WriteLine("Adresa nije pronadjena");
                return;
            }
            System.Console.WriteLine("Adresa azurirana");
        }

        private void RemoveAdresa()
        {
            int id = InputId();
            Adresa removedAdresa = adresaManager.RemoveAdresa(id);

            if (removedAdresa == null)
            {
                System.Console.WriteLine("Adresa nije pronadjena");
                return;
            }

            System.Console.WriteLine("Adresa uklonjena");
        }

        private void AddAdresa()
        {
            Adresa adresa = InputAdresa();
            adresaManager.addAdresa(adresa);
            System.Console.WriteLine("Adresa dodata");
        }

        public void RunAdresaMenu()
        {
            while (true)
            {
                ShowAdresaMenu();
                string userInput = System.Console.ReadLine();
                if (userInput == "0")
                {
                    break;
                }
                HandleAdresaMenuInput(userInput);
            }
        }

        public void HandleAdresaMenuInput(string input)
        {
            switch (input)
            {
                case "1":
                    PrintAdrese(adresaManager.GetAllAdrese());
                    break;
                case "2":
                    AddAdresa();
                    break;
                case "3":
                    UpdateAdresa();
                    break;
                case "4":
                    RemoveAdresa();
                    break;

            }
        }

        public void ShowAdresaMenu()
        {
            System.Console.WriteLine("\nIzaberi opciju:");
            System.Console.WriteLine("1: Prikazi sve adrese");
            System.Console.WriteLine("2: Dodaj adresu");
            System.Console.WriteLine("3: Azuriraj adresu");
            System.Console.WriteLine("4: Ukloni adresu");
            System.Console.WriteLine("0: Back to main menu");
        }
    }
}
