using System;
using System.Collections.Generic;
using System.Text;
using Projekat.Model;
using Projekat.Manager;
using System.Linq;

namespace Projekat.Console
{
    class StudentConsoleView : ConsoleView
    {
        private StudentManager manager;

        public StudentConsoleView(StudentManager manager)
        {
            this.manager = manager;
        }

        public void PrintStudenti(List<Student> studenti)
        {
            System.Console.WriteLine("Studenti: ");
            foreach (Student student in studenti)
            {
                System.Console.WriteLine(student);
            }    
        }

        private Student InputStudent()
        {
            Student student = new Student();

            System.Console.Write("Unesi ime: ");
            string ime = System.Console.ReadLine();
            student.ime = ime;

            System.Console.Write("Unesi prezime: ");
            string prezime = System.Console.ReadLine();
            student.prezime = prezime;

            while(true)
            {
                System.Console.Write("Unesi datum rodjenja: ");
                try
                {
                    DateTime datumRodjenja = Convert.ToDateTime(System.Console.ReadLine());
                    student.datumRodjenja = datumRodjenja;

                    if(datumRodjenja > DateTime.Now)
                    {
                        System.Console.WriteLine("Uneti datum je u buducnosti. Ponovi unos");
                    }
                    else
                        break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format datuma nije ispravan. Ponovi unos");
                }
            }

            while (true)
            {
                System.Console.Write("Unesi id adrese (unesi -1 ako adresa nije poznata): ");
                try
                {
                    int adresaId = Convert.ToInt32(System.Console.ReadLine());
                    if (manager.parentManager.adrese.Find(a => a.Id == adresaId) == null && adresaId != -1)
                    {
                        System.Console.WriteLine("Uneta adresa ne postoji");
                        continue;
                    }
                    if (adresaId == -1)
                    {
                        break;
                    }
                    student.adresaId = adresaId;
                    break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format id-ja nije ispravan. Ponovi unos");
                }
            }
            foreach (Adresa adresa in manager.parentManager.adrese)
            {
                if (adresa == null) continue;
                if (adresa.Id == student.adresaId)
                {
                    student.adresa = adresa;
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
                    if ((i < kontakt.Length) && (kontakt[i] < '0' || kontakt[i] > '9'))
                    {
                        System.Console.WriteLine("Nije unet dobar format kontakta. Ponovite unos.");
                        break;
                    }

                    if (i == kontakt.Length)
                    {
                        neispravanUnosKontakta = false;
                        student.brTelefona = kontakt;
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
                student.email = emailAdresa;
                break;
            }

            while (true)
            {
                System.Console.Write("Unesi broj indeksa (format RAxx-xxxx): ");
                string brIndeksa = System.Console.ReadLine();
                int i = 0;
                bool greska = false;
                foreach (char c in brIndeksa)
                {
                    if (i < 2)
                    {
                        if (!char.IsLetter(c))
                        {
                            System.Console.WriteLine("Neispravan format za broj indeksa. Ponovi unos");
                            greska = true;
                            break;
                        }
                    }
                    else if (i >= 2 && i < 4)
                    {
                        if (!char.IsDigit(c))
                        {
                            System.Console.WriteLine("Neispravan format za broj indeksa. Ponovi unos");
                            greska = true;
                            break;
                        }    
                    }
                    else if (i == 4)
                    {
                        if (c != '-')
                        {
                            System.Console.WriteLine("Neispravan format za broj indeksa. Ponovi unos");
                            greska = true;
                            break;
                        }
                    }
                    else
                    {
                        if (!char.IsDigit(c))
                        { 
                            System.Console.WriteLine("Neispravan format za broj indeksa. Ponovi unos");
                            greska = true;
                            break;
                        }
                    }
                    i++;
                }
                if (brIndeksa.Length != 9 && !greska)
                {
                    System.Console.WriteLine("Neispravan format za broj indeksa. Ponovi unos");
                    greska = true;
                }
                if (greska) continue;
                student.brIndeksa = brIndeksa;
                break;
            }

            while (true)
            {
                System.Console.Write("Unesi godinu upisa: ");
                try
                {
                    uint godinaUpisa = Convert.ToUInt32(System.Console.ReadLine());
                    student.godinaUpisa = godinaUpisa;
                    DateTime dateTime = DateTime.Now;
                    if (dateTime.Year < godinaUpisa)
                    {
                        System.Console.WriteLine("Uneta godina je u buducnosti. Ponovi unos");
                    }
                    else
                        break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format godine nije ispravan. Ponovi unos");
                }
            }

            while (true)
            {
                System.Console.Write("Unesi godinu studija: ");
                try
                {
                    uint godinaStudija = Convert.ToUInt32(System.Console.ReadLine());
                    student.godinaStudija = godinaStudija;
                    if (godinaStudija < 1 || godinaStudija > 4)
                    {
                        System.Console.WriteLine("Nije uneta ispravna vrednost. Ponovi unos");
                    }
                    else
                        break;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Format godine nije ispravan. Ponovi unos");
                }
            }

            string status;
            do
            {
                System.Console.Write("Unesi status (B ili S): ");
                status = System.Console.ReadLine();
                switch (status)
                {
                    case "B":
                        student.status = Status.B;
                        break;
                    case "S":
                        student.status = Status.S;
                        break;
                    case "b":
                        student.status = Status.B;
                        break;
                    case "s":
                        student.status = Status.S;
                        break;
                    default:
                        System.Console.WriteLine("Uneta je nedozvoljena vrednost. Ponovi unos");
                        break;
                }
            }
            while (status != "B" && status != "b" && status != "S" && status != "s");

            //polozeni predmeti
            List<int> oceneId = new List<int>();
            System.Console.WriteLine("Unesi id ocena koje pripadaju studentu: ");
            for (int i = 0; true;)
            {
                try
                {
                    System.Console.Write("Unesi id ili (-1) za zavrsetak unosa: ");
                    int temp = Convert.ToInt32(System.Console.ReadLine());
                    if (manager.parentManager.ocene.Find(o => o.Id == temp) == null && temp != -1)
                    {
                        System.Console.WriteLine("Uneta ocena ne postoji");
                        continue;
                    }
                    oceneId.Add(temp);
                    if (oceneId[i] == -1)
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
            oceneId = oceneId.Distinct().ToList();
            foreach (int ocenaId in oceneId)
            {
                Ocena ocena = manager.parentManager.ocene.Find(o => o.Id == ocenaId);
                if (ocena == null) continue;
                student.polozeniPredmeti.Add(ocena);
            }

            // nepolozeni predmeti
            List<int> predmetiId = new List<int>();
            System.Console.WriteLine("Unesi sifru predmeta koje student nije polozio: ");
            for (int i = 0; true;)
            {
                try
                {
                    System.Console.Write("Unesi id ili (-1) za zavrsetak unosa: ");
                    int temp = Convert.ToInt32(System.Console.ReadLine());
                    if (manager.parentManager.predmeti.Find(p => p.id == temp) == null && temp != -1)
                    {
                        System.Console.WriteLine("Uneti predmet ne postoji");
                        continue;
                    }
                    predmetiId.Add(temp);
                    if (predmetiId[i] == -1)
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
            predmetiId = predmetiId.Distinct().ToList();
            foreach (int predmetId in predmetiId)
            {
                Predmet predmet = manager.parentManager.predmeti.Find(p => p.id == predmetId);
                if (predmet == null) continue;
                student.nepolozeniPredmeti.Add(predmet);
            }

            return student;
        }

        private int InputId()
        {
            System.Console.Write("Unesi id studenta: ");
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
                    PrintStudenti(manager.parentManager.studenti);
                    break;
                case "2":
                    AddStudent();
                    break;
                case "3":
                    UpdateStudent();
                    break;
                case "4":
                    RemoveStudent();
                    break;

            }
        }

        private void RemoveStudent()
        {
            int id = InputId();
            Student removedStudent = manager.RemoveStudent(id);
            if (removedStudent == null)
            {
                System.Console.WriteLine("Student nije pronadjen");
                return;
            }
            System.Console.WriteLine("Student uklonjen");
        }

        private void UpdateStudent()
        {
            int id = InputId();
            Student student = InputStudent();
            student.id = id;

            student.prosecnaOcena = 0;
            foreach (Ocena ocena in student.polozeniPredmeti)
            {
                student.prosecnaOcena += ocena.vrednost;
            }

            if (student.polozeniPredmeti.Count != 0)
            {
                student.prosecnaOcena /= student.polozeniPredmeti.Count;
            }

            Student updatedStudent = manager.updateStudent(student);
            if (updatedStudent == null)
            {
                System.Console.WriteLine("Student nije pronadjen");
                return;
            }
            System.Console.WriteLine("Student azuriran");
        }

        private void AddStudent()
        {
            Student student = InputStudent();

            student.prosecnaOcena = 0;
            
            foreach (Ocena ocena in student.polozeniPredmeti)
            {
                student.prosecnaOcena += ocena.vrednost;
            }

            if (student.polozeniPredmeti.Count != 0)
            {
                student.prosecnaOcena /= student.polozeniPredmeti.Count;
            }

            manager.addStudent(student);
            System.Console.WriteLine("Student dodat");
        }

        private void ShowMenu()
        {
            System.Console.WriteLine("\nIzaberi opciju: ");
            System.Console.WriteLine("1: Prikazi sve studente");
            System.Console.WriteLine("2: Dodaj studenta");
            System.Console.WriteLine("3: Azuriraj studenta");
            System.Console.WriteLine("4: Ukloni studenta");
            System.Console.WriteLine("0: Izlaz");
            System.Console.Write("Izbor: ");
        }
    }
}