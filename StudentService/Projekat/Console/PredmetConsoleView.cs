using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Projekat.Manager;

namespace Projekat.Console
{
    class PredmetConsoleView : ConsoleView
    {
        private PredmetManager manager;

        public PredmetConsoleView(PredmetManager manager)
        {
            this.manager = manager;
        }

        public void PrintPredmeti(List<Predmet> predmeti)
        {
            System.Console.WriteLine("Predmeti: ");
            //string header = string.Format("Sifra {0,1} | Naziv: {0,30} | Semestar: {0,6} | Profesor: {0,30} | ESPB: {0,2} |", "");
            //System.Console.WriteLine(header);
            foreach (Predmet predmet in predmeti)
            {
                System.Console.WriteLine(predmet);
            }
        }

        private Predmet InputPredmet()
        {
            Predmet predmet = new Predmet();

            System.Console.Write("Unesi naziv: ");
            string naziv = System.Console.ReadLine();
            predmet.naziv = naziv;

            string semestar;
            do
            {
                System.Console.Write("Unesi semestar (zimski ili letnji): ");
                semestar = System.Console.ReadLine();
                semestar = semestar.ToLower();
                switch (semestar)
                {
                    case "zimski":
                        predmet.semestar = Semestar.Zimski;
                        break;
                    case "letnji":
                        predmet.semestar = Semestar.Letnji;
                        break;
                    default:
                        System.Console.WriteLine("Uneta je nedozvoljena vrednost. Ponovi unos");
                        break;
                }
            }
            while (semestar != "zimski" && semestar != "letnji");

            while (true)
            {
                System.Console.Write("Unesi id profesora (unesi -1 ako profesor nije poznat): ");
                try
                {
                    int profesorId = Convert.ToInt32(System.Console.ReadLine());
                    if (manager.parentManager.profesori.Find(p => p.Id == profesorId) == null && profesorId != -1)
                    {
                        System.Console.WriteLine("Uneti profesor ne postoji");
                        continue;
                    }
                    if (profesorId == -1)
                    {
                        break;
                    }
                    predmet.profesorId = profesorId;
                    break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format id-ja nije ispravan. Ponovi unos");
                }
            }
            foreach (Profesor profesor in manager.parentManager.profesori)
            {
                if (profesor == null) continue;
                if (profesor.Id == predmet.profesorId)
                {
                    predmet.profesor = profesor;
                }
            }

            while (true)
            {
                System.Console.Write("Unesi broj ESPB koje predmet nosi: ");
                try
                {
                    uint espb = Convert.ToUInt32(System.Console.ReadLine());
                    predmet.espb = espb;
                    break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format ESPB nije ispravan. Ponovi unos");
                }
            }

            List<int> poloziliId = new List<int>();
            System.Console.WriteLine("Unesi id studenata koji su polozili predmet: ");
            for (int i = 0; true;)
            {
                try
                {
                    System.Console.Write("Unesi id ili (-1) za zavrsetak unosa: ");
                    int temp = Convert.ToInt32(System.Console.ReadLine());
                    if (manager.parentManager.studenti.Find(s => s.id == temp) == null && temp != -1)
                    {
                        System.Console.WriteLine("Uneti student ne postoji");
                        continue;
                    }
                    poloziliId.Add(temp);
                    if (poloziliId[i] == -1)
                    {
                        break;
                    }
                    i++;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format id-ja nije ispravan. Ponovi unos");
                }
            }
            poloziliId = poloziliId.Distinct().ToList();
            foreach (int studentId in poloziliId)
            {
                Student student = manager.parentManager.studenti.Find(s => s.id == studentId);
                if (student == null) continue;
                predmet.polozili.Add(student);
            }

            List<int> nisuPoloziliId = new List<int>();
            System.Console.WriteLine("Unesi id studenta koji nije polozio predmet: ");
            for (int i = 0; true;)
            {
                try
                {
                    System.Console.Write("Unesi id ili (-1) za zavrsetak unosa: ");
                    int temp = Convert.ToInt32(System.Console.ReadLine());
                    if (manager.parentManager.studenti.Find(s => s.id == temp) == null && temp != -1)
                    {
                        System.Console.WriteLine("Uneti student ne postoji");
                        continue;
                    }
                    nisuPoloziliId.Add(temp);
                    if (nisuPoloziliId[i] == -1)
                    {
                        break;
                    }
                    i++;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format id-ja nije ispravan. Ponovi unos");
                }
            }
            nisuPoloziliId = nisuPoloziliId.Distinct().ToList();
            foreach (int studentId in nisuPoloziliId)
            {
                Student student = manager.parentManager.studenti.Find(s => s.id == studentId);
                if (student == null) continue;
                predmet.nisuPolozili.Add(student);
            }

            return predmet;
        }

        private int InputId()
        {
            System.Console.Write("Unesi sifru predmeta: ");
            return SafeInputInt();
        }

        public void RunMenu()
        {
            while (true)
            {
                ShowMenu();
                string userInput = System.Console.ReadLine();
                if (userInput == "0") break;
                HandleMenuInput(userInput);
            }
        }

        private void HandleMenuInput(string input)
        {
            switch (input)
            {
                case "1":
                    PrintPredmeti(manager.parentManager.predmeti);
                    break;
                case "2":
                    AddPredmet();
                    break;
                case "3":
                    UpdatePredmet();
                    break;
                case "4":
                    RemovePredmet();
                    break;

            }
        }

        private void RemovePredmet()
        {
            int id = InputId();
            Predmet removedPredmet = manager.RemovePredmet(id);
            if (removedPredmet == null)
            {
                System.Console.WriteLine("Predmet nije pronadjen");
                return;
            }
            System.Console.WriteLine("Predmet uklonjen");
        }

        private void UpdatePredmet()
        {
            int id = InputId();
            Predmet predmet = InputPredmet();
            predmet.id = id;

            Predmet updatedPredmet = manager.updatePredmet(predmet);
            if (updatedPredmet == null)
            {
                System.Console.WriteLine("Predmet nije pronadjen");
                return;
            }
            System.Console.WriteLine("Predmet azuriran");
        }

        private void AddPredmet()
        {
            Predmet predmet = InputPredmet();
            manager.addPredmet(predmet);
            System.Console.WriteLine("Predmet dodat");
        }

        private void ShowMenu()
        {
            System.Console.WriteLine("\nIzaberi opciju: ");
            System.Console.WriteLine("1: Prikazi sve predmete");
            System.Console.WriteLine("2: Dodaj predmet");
            System.Console.WriteLine("3: Azuriraj predmet");
            System.Console.WriteLine("4: Ukloni predmet");
            System.Console.WriteLine("0: Izlaz");
            System.Console.Write("Izbor: ");
        }
    }
}
