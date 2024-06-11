//Kod preuzet iz materijala sa vezbi
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Manager.Serialization
{
    public interface Serializable
    {
        string[] ToCSV();

        void FromCSV(string[] values);
    }
}
