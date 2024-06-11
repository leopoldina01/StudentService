using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projekat.Manager;
using Projekat.Model;

namespace Projekat.Console
{
    class ProfesorConsoleView : ConsoleView
    {
        private ProfesorManager profesorManager;

        public ProfesorConsoleView(ProfesorManager profesorManager)
        {
            this.profesorManager = profesorManager;
        }

        private void PrintProfessors(List<Profesor> profesori)
        {
            System.Console.WriteLine("Profesori: ");

            foreach (Profesor p in profesori)
            {
                System.Console.WriteLine(p);
            }
        }

        private Profesor InputProfesor()
        {
            Profesor profesor = new Profesor();

            System.Console.WriteLine("Unesi Prezime: ");
            string prezime = System.Console.ReadLine();
            profesor.prezime = prezime;

            System.Console.WriteLine("Unesi ime: ");
            string ime = System.Console.ReadLine();
            profesor.ime = ime;

            while (true)
            {
                System.Console.WriteLine("Unesi datum rodjenja (mm.dd.gggg.): ");
                try
                {
                    DateTime datumRodj = Convert.ToDateTime(System.Console.ReadLine());
                    profesor.datumRodj = datumRodj;
                    if (datumRodj > DateTime.Now)
                    {
                        System.Console.WriteLine("Uneti datum je u buducnosti. Ponovi unos");
                    }
                    break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format datuma nije ispravan. Ponovi unos");
                }
            }

            while (true)
            {
                System.Console.Write("Unesi id adrese stana (unesi -1 ako adresa nije poznata): ");
                try
                {
                    int adresaStanId = Convert.ToInt32(System.Console.ReadLine());
                    if (profesorManager.parentManager.adrese.Find(a => a.Id == adresaStanId) == null && adresaStanId != -1)
                    {
                        System.Console.WriteLine("Uneta adresa ne postoji");
                        continue;
                    }
                    if (adresaStanId == -1)
                    {
                        break;
                    }
                    profesor.adresaStanId = adresaStanId;
                    break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format id-ja nije ispravan. Ponovi unos");
                }
            }
            foreach (Adresa adresa in profesorManager.parentManager.adrese)
            {
                if (adresa == null)
                {
                    continue;
                }
                if (adresa.Id == profesor.adresaStanId)
                {
                    profesor.adresaStan = adresa;
                }
            }

            bool neispravanUnosKontakta = true;
            while (neispravanUnosKontakta)
            {
                System.Console.WriteLine("Unesi kontakt telefon (+38... ili 06...): ");
                string kontakt = System.Console.ReadLine();

                if (kontakt.Length < 9)
                {
                    System.Console.WriteLine("Kontakt nema dovoljno cifara. Ponovite unos");
                    continue;
                }
                
                int i = 0;
                while (i != kontakt.Length)
                {
                    if ((i == 0) && kontakt[i] != '+')
                    {
                        if (kontakt.Length > 10)
                        {
                            System.Console.WriteLine("Kontakt sadrzi previse cifara. Ponovite unos");
                            break;
                        }

                        if (kontakt[i] != '0' || kontakt[i + 1] != '6')
                        {
                            System.Console.WriteLine("Nije unet dobar format kontakta. Ponovite unos.");
                            break;
                        }

                        i = 1;
                    }

                    if ((i == 0) && kontakt[i] == '+')
                    {
                        if (kontakt.Length < 12 || kontakt.Length > 13)
                        {
                            System.Console.WriteLine("Nije unet dobar format kontakta. Ponovite unos.");
                            break;
                        }

                        if (kontakt[1] != '3' || kontakt[2] != '8')
                        {
                            System.Console.WriteLine("Nije unet dobar format kontakta. Ponovite unos.");
                            break;
                        }
                        i = 2;
                    }

                    i++;
                    if ((i < kontakt.Length) && (kontakt[i] <'0' || kontakt[i] > '9'))
                    {
                        System.Console.WriteLine("Nije unet dobar format kontakta. Ponovite unos.");
                        break;
                    }

                    if (i == kontakt.Length)
                    {
                        neispravanUnosKontakta = false;
                        profesor.kontakt = kontakt;
                    }
                }
            }

            while (true)
            {
                System.Console.WriteLine("Unesi e-mail adresu (da se zavrsava sa @gmail.com ili @uns.ac.rs): ");
                string emailAdresa = System.Console.ReadLine();
                if (!emailAdresa.EndsWith("@gmail.com") && !emailAdresa.EndsWith("@uns.ac.rs"))
                {
                    System.Console.WriteLine("e-mail adresa se mora zavrsiti sa @gmail.com ili @uns.ac.rs. Ponovi unos");
                    continue;
                }
                profesor.email = emailAdresa;
                break;
            }

            while (true)
            {
                System.Console.Write("Unesi id adrese kancelarije (unesi -1 ako adresa nije poznata): ");
                try
                {
                    int adresaKancId = Convert.ToInt32(System.Console.ReadLine());
                    if (profesorManager.parentManager.adrese.Find(a => a.Id == adresaKancId) == null && adresaKancId != -1)
                    {
                        System.Console.WriteLine("Uneta adresa ne postoji");
                        continue;
                    }
                    if (adresaKancId == -1)
                    {
                        break;
                    }
                    profesor.adresaKancId = adresaKancId;
                    break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format id-ja nije ispravan. Ponovi unos");
                }
            }
            foreach (Adresa adresa in profesorManager.parentManager.adrese)
            {
                if (adresa == null)
                { 
                    continue; 
                }
                if (adresa.Id == profesor.adresaKancId)
                {
                    profesor.adresaKanc = adresa;
                }
            }

            while (true)
            {
                System.Console.WriteLine("Unesi brlk: ");
                string brlk = System.Console.ReadLine();

                if (brlk.Length != 9)
                {
                    System.Console.WriteLine("BRLK mora sadrzati 9 cifara. Ponovi unos");
                    continue;
                }

                int i = 0;
                while (i < brlk.Length)
                {
                    if (!Char.IsDigit(brlk[i]))
                    {
                        System.Console.WriteLine("BRLK mora sadrzati samo cifre. Ponovi unos");
                        break;
                    }
                    i++;
                }

                if (i == brlk.Length)
                {
                    profesor.brlk = brlk;
                    break;
                }

            }

            System.Console.WriteLine("Unesi zvanje profesora: ");
            string zvanje = System.Console.ReadLine();
            profesor.zvanje = zvanje;

            System.Console.WriteLine("Unesi godine staza profesora: ");
            int godineStaza = SafeInputInt();
            profesor.godineStaza = godineStaza;

            //predmeti koje profesor predaje
            List<int> predmetiId = new List<int>();
            System.Console.WriteLine("Unesi id predmeta koji profesor predaje: ");
            for (int i = 0; true;)
            {
                try
                {
                    System.Console.Write("Unesi id ili (-1) za zavrsetak unosa: ");
                    int temp = SafeInputInt();
                    if (profesorManager.parentManager.predmeti.Find(p => p.id == temp) == null && temp != -1)
                    {
                        System.Console.WriteLine("Uneti predmet ne postoji");
                        continue;
                    }
                    if (temp == -1)
                    {
                        break;
                    }
                    predmetiId.Add(temp);
                    i++;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format id-ja nije ispravan. Ponovi unos");
                }
            }

            predmetiId = predmetiId.Distinct().ToList();
            foreach (int predmetId in predmetiId)
            {
                Predmet predmet = profesorManager.parentManager.predmeti.Find(p => p.id == predmetId);
                if (predmet == null)
                {
                    continue;
                }
                profesor.predmeti.Add(predmet);
            }

            return profesor;
        }

        private void UpdateProfesor()
        {
            int id = InputId();
            Profesor profesor = InputProfesor();
            profesor.Id = id;
            Profesor updatedProfesor = profesorManager.UpdateProfesor(profesor);
            if (updatedProfesor == null)
            {
                System.Console.WriteLine("Profesor nije pronadjen");
                return;
            }
            System.Console.WriteLine("Profesor azuriran");
        }

        private void RemoveProfesor()
        {
            int id = InputId();
            Profesor removedProfesor = profesorManager.RemoveProfesor(id);

            if (removedProfesor == null)
            {
                System.Console.WriteLine("Profesor nije pronadjen");
                return;
            }

            System.Console.WriteLine("Profesor uklonjen");
        }
        

        private void AddProfesor()
        {
            Profesor profesor = InputProfesor();
            profesorManager.AddProfesor(profesor);
            System.Console.WriteLine("Profesor dodat");
        }

        private int InputId()
        {
            System.Console.WriteLine("Unesi id profesora: ");
            return SafeInputInt();
        }

        public void RunProfesorMenu()
        {
            while (true)
            {
                ShowProfesorMenu();
                string userInput = System.Console.ReadLine();
                if (userInput == "0")
                {
                    break;
                }
                HandleProfesorMenuInput(userInput);
            }
        }

        public void HandleProfesorMenuInput(string input)
        {
            switch (input)
            {
                case "1":
                    PrintProfessors(profesorManager.GetAllProfessors());
                    break;
                case "2":
                    AddProfesor();
                    break;
                case "3":
                    UpdateProfesor();
                    break;
                case "4":
                    RemoveProfesor();
                    break;

            }
        }

        public void ShowProfesorMenu()
        {
            System.Console.WriteLine("\nIzaberi opciju:");
            System.Console.WriteLine("1: Prikazi sve profesore");
            System.Console.WriteLine("2: Dodaj profesora");
            System.Console.WriteLine("3: Izmeni profesora");
            System.Console.WriteLine("4: Ukloni profesora");
            System.Console.WriteLine("0: Back to main menu");
        }
    }
}
