using System;
using System.Collections.Generic;
using System.Text;
using Projekat.Manager;
using Projekat.Model;

namespace Projekat.Console
{
    class OcenaConsoleView : ConsoleView
    {
        private OcenaManager ocenaManager;

        public OcenaConsoleView(OcenaManager ocenaManager)
        {
            this.ocenaManager = ocenaManager;
        }

        public void PrintOcene(List<Ocena> ocene)
        {
            ocenaManager.saveOcene();
            System.Console.WriteLine("Ocene: ");

            foreach (Ocena ocena in ocene)
            {
                System.Console.WriteLine(ocena);
            }

        }

        public Ocena InputOcena()
        {
            Ocena ocena = new Ocena();
            while (true)
            {
                System.Console.Write("Unesi id studenta (ili unesi -1 ako student nije poznat): ");
                try
                {
                    int polozioId = Convert.ToInt32(System.Console.ReadLine());
                    if (ocenaManager.parentManager.studenti.Find(s => s.id == polozioId) == null && polozioId != -1)
                    {
                        System.Console.WriteLine("Uneti student ne postoji");
                        continue;
                    }
                    if (polozioId == -1)
                    {
                        break;
                    }
                    ocena.polozioId = polozioId;
                    break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format id-ja nije ispravan. Ponovi unos");
                }
            }
            foreach (Student student in ocenaManager.parentManager.studenti)
            {
                if (student == null)
                {
                    continue;
                }
                if (student.id == ocena.polozioId)
                {
                    ocena.polozio = student;
                }
            }

            while (true)
            {
                System.Console.Write("Unesi id predmeta (ili unesi -1 ako predmet nije poznat): ");
                try
                {
                    int predmetId = Convert.ToInt32(System.Console.ReadLine());
                    if (ocenaManager.parentManager.predmeti.Find(p => p.id == predmetId) == null && predmetId != -1)
                    {
                        System.Console.WriteLine("Uneti predmet ne postoji");
                        continue;
                    }
                    if (predmetId == -1)
                    {
                        break;
                    }
                    ocena.predmetId = predmetId;
                    break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format id-ja nije ispravan. Ponovi unos");
                }
            }
            foreach (Predmet predmet in ocenaManager.parentManager.predmeti)
            {
                if (predmet == null)
                {
                    continue;
                }
                if (predmet.id == ocena.predmetId)
                {
                    ocena.predmet = predmet;
                }
            }

            while (true)
            {
                System.Console.Write("Unesi vrednost ocene (5 - 10): ");
                try
                {
                    double vrednost = Convert.ToDouble(System.Console.ReadLine());
                    ocena.vrednost = vrednost;
                    if (vrednost < 5 || vrednost > 10)
                    {
                        System.Console.WriteLine("Vrednost ocene mora biti u opsegu 5-10. Ponovi unos");
                        continue;
                    }
                    break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format ocene nije ispravan. Ponovi unos");
                }
            }

            while (true)
            {
                System.Console.WriteLine("Unesi datum polaganja (mm.dd.gggg.): ");
                try
                {
                    DateTime datumPolaganja = Convert.ToDateTime(System.Console.ReadLine());
                    ocena.datumPolaganja = datumPolaganja;
                    break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format datuma nije ispravan. Ponovi unos");
                }
            }

            return ocena;
        }

        private void UpdateOcena()
        {
            int id = InputId();
            Ocena ocena = InputOcena();
            ocena.Id = id;
            Ocena updatedOcena = ocenaManager.updateOcena(ocena);
            if (updatedOcena == null)
            {
                System.Console.WriteLine("Ocena nije pronadjena");
                return;
            }
            System.Console.WriteLine("Ocena azurirana");
        }

        private void RemoveOcena()
        {
            int id = InputId();
            Ocena removedOcena = ocenaManager.RemoveOcena(id);

            if (removedOcena == null)
            {
                System.Console.WriteLine("Ocena nije pronadjena");
                return;
            }

            System.Console.WriteLine("Ocena uklonjena");
        }

        private void AddOcena()
        {
            Ocena ocena = InputOcena();
            ocenaManager.addOcena(ocena);
            System.Console.WriteLine("Ocena dodata");
        }

        private int InputId()
        {
            System.Console.WriteLine("Unesi id ocene: ");
            return SafeInputInt();
        }

        public void RunOcenaMenu()
        {
            while (true)
            {
                ShowOcenaMenu();
                string userInput = System.Console.ReadLine();
                if (userInput == "0")
                {
                    break;
                }
                HandleOcenaMenuInput(userInput);
            }
        }

        public void HandleOcenaMenuInput(string input)
        {
            switch (input)
            {
                case "1":
                    PrintOcene(ocenaManager.GetAllOcene());
                    break;
                case "2":
                    AddOcena();
                    break;
                case "3":
                    UpdateOcena();
                    break;
                case "4":
                    RemoveOcena();
                    break;

            }
        }

        public void ShowOcenaMenu()
        {
            System.Console.WriteLine("\nIzaberi opciju:");
            System.Console.WriteLine("1: Prikazi sve ocene");
            System.Console.WriteLine("2: Dodaj ocenu");
            System.Console.WriteLine("3: Azuriraj ocenu");
            System.Console.WriteLine("4: Ukloni ocenu");
            System.Console.WriteLine("0: Back to main menu");
        }
    }
}
