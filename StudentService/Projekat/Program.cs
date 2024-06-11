/* https://docs.google.com/document/d/13IkFq1gaWj6gI8FtOH1QQSvKXdx0VVXqKCBkhOL-bH8/edit#heading=h.stmyt7wlqlrd
using Projekat.Manager;
using Projekat.Model;
using Projekat.Console;
using System;
//original
namespace Projekat
{
    class Program
    {
        static void Main(string[] args)
        {

            ParentManager parentManager = new ParentManager();

            ProfesorManager profesorManager = new ProfesorManager(parentManager);
            ProfesorConsoleView profesorView = new ProfesorConsoleView(profesorManager);

            StudentManager studentManager = new StudentManager(parentManager);
            StudentConsoleView studentView = new StudentConsoleView(studentManager);

            OcenaManager ocenaManager = new OcenaManager(parentManager);
            OcenaConsoleView ocenaView = new OcenaConsoleView(ocenaManager);

            PredmetManager predmetManager = new PredmetManager(parentManager);
            PredmetConsoleView predmetView = new PredmetConsoleView(predmetManager);

            KatedraManager katedraManager = new KatedraManager(parentManager);
            KatedraConsoleView katedraView = new KatedraConsoleView(katedraManager);

            AdresaManager adresaManager = new AdresaManager(parentManager);
            AdresaConsoleView adresaView = new AdresaConsoleView(adresaManager);            

            while (true)
            {
                System.Console.WriteLine("\nIzaberi opciju:\n");
                System.Console.WriteLine("1: Student Menu");
                System.Console.WriteLine("2: Profesor Menu");
                System.Console.WriteLine("3: Predmet Menu");
                System.Console.WriteLine("4: Ocena Menu");
                System.Console.WriteLine("5: Katedra Menu");
                System.Console.WriteLine("6: Adresa Menu");
                System.Console.WriteLine("0: Izadji iz studentske sluzbe");
                System.Console.Write("Izbor: ");
                string userInput = System.Console.ReadLine();
                if (userInput == "0")
                {
                    break;
                }
                switch (userInput)
                {
                    case "1":
                        studentView.RunMenu();
                        break;
                    case "2":
                        profesorView.RunProfesorMenu();
                        break;
                    case "3":
                        predmetView.RunMenu();
                        break;
                    case "4":
                        ocenaView.RunOcenaMenu();
                        break;
                    case "5":
                        katedraView.RunMenu();
                        break;
                    case "6":
                        adresaView.RunAdresaMenu();
                        break;
                }
            }
        }
    }
}
*/