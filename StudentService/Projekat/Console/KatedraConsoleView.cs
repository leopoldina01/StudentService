using Projekat.Manager;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Console
{
    class KatedraConsoleView : ConsoleView
    {
        private KatedraManager manager;

        public KatedraConsoleView(KatedraManager manager)
        {
            this.manager = manager;
        }

        public void PrintKatedra(List<Katedra> katedre)
        {
            System.Console.WriteLine("Katedre: ");
            //string header = string.Format("Sifra: {0,2} | Naziv: {0,20} | Sef: {0,20} | Profesori: {0,20} | ", "");
            //System.Console.WriteLine(header);
            foreach (Katedra katedra in katedre)
            {
                System.Console.WriteLine(katedra);
            }
        }

        private Katedra InputKatedra()
        {
            Katedra katedra = new Katedra();

            System.Console.Write("Unesi naziv: ");
            string naziv = System.Console.ReadLine();
            katedra.naziv = naziv;

            while (true)
            {
                System.Console.Write("Unesi id profesora sefa katedre (unesi -1 ako adresa nije poznata): ");
                try
                {
                    int sefId = Convert.ToInt32(System.Console.ReadLine());
                    if (manager.parentManager.profesori.Find(p => p.Id == sefId) == null && sefId != -1)
                    {
                        System.Console.WriteLine("Uneti profesor ne postoji");
                        continue;
                    }
                    if (sefId == -1)
                    {
                        break;
                    }
                    katedra.sefId = sefId;
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
                if (profesor.Id == katedra.sefId)
                {
                    katedra.sef = profesor;
                }
            }

            List<int> profesoriId = new List<int>();
            System.Console.WriteLine("Unesi id profesora koji pripadaju katedri: ");
            for (int i = 0; true;)
            {
                try
                {
                    System.Console.Write("Unesi id ili (-1) za zavrsetak unosa: ");
                    int temp = Convert.ToInt32(System.Console.ReadLine());
                    if (manager.parentManager.profesori.Find(p => p.Id == temp) == null && temp != -1)
                    {
                        System.Console.WriteLine("Uneti profesor ne postoji");
                        continue;
                    }
                    profesoriId.Add(temp);
                    if (profesoriId[i] == -1)
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
            profesoriId = profesoriId.Distinct().ToList();
            foreach (int profesorId in profesoriId)
            {
                Profesor profesor = manager.parentManager.profesori.Find(p => p.Id == profesorId);
                if (profesor == null) continue;
                katedra.profesori.Add(profesor);
            }

            return katedra;
        }

        private int InputId()
        {
            System.Console.Write("Unesi sifru katedre: ");
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
                    PrintKatedra(manager.parentManager.katedre);
                    break;
                case "2":
                    AddKatedra();
                    break;
                case "3":
                    UpdateKatedra();
                    break;
                case "4":
                    RemoveKatedra();
                    break;

            }
        }

        private void RemoveKatedra()
        {
            int id = InputId();
            Katedra removedKatedra = manager.RemoveKatedra(id);
            if (removedKatedra == null)
            {
                System.Console.WriteLine("Katedra nije pronadjena");
                return;
            }
            System.Console.WriteLine("Katedra uklonjena");
        }

        private void UpdateKatedra()
        {
            int id = InputId();
            Katedra katedra = InputKatedra();
            katedra.id = id;

            Katedra updatedKatedra = manager.updateKatedra(katedra);
            if (updatedKatedra == null)
            {
                System.Console.WriteLine("Katedra nije pronadjena");
                return;
            }
            System.Console.WriteLine("Katedra azurirana");
        }

        private void AddKatedra()
        {
            Katedra katedra = InputKatedra();

            manager.addKatedra(katedra);
            System.Console.WriteLine("Katedra dodata");
        }

        private void ShowMenu()
        {
            System.Console.WriteLine("\nIzaberi opciju: ");
            System.Console.WriteLine("1: Prikazi sve katedre");
            System.Console.WriteLine("2: Dodaj katedru");
            System.Console.WriteLine("3: Azuriraj katedru");
            System.Console.WriteLine("4: Ukloni katedru");
            System.Console.WriteLine("0: Izlaz");
            System.Console.Write("Izbor: ");
        }
    }
}
