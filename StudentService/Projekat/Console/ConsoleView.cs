// Kod preuzet iz materijala sa vezbi.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Console
{
    class ConsoleView
    {
        protected int SafeInputInt()
        {
            int input;

            string rawInput = System.Console.ReadLine();

            while (!int.TryParse(rawInput, out input))
            {
                System.Console.WriteLine("Not a valid number, try again: ");

                rawInput = System.Console.ReadLine();
            }
            return input;
        }
    }
}
