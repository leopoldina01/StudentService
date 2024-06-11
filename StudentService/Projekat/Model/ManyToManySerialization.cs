using System;
using System.Collections.Generic;
using System.Text;
using Projekat.Manager.Serialization;

namespace Projekat.Model
{
    class ManyToManySerialization : Serializable
    {
        public int id1 { get; set; }
        public int id2 { get; set; }

        public ManyToManySerialization()
        {

        }

        public ManyToManySerialization(int id1, int id2)
        {
            this.id1 = id1;
            this.id2 = id2;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                id1.ToString(),
                id2.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            id1 = int.Parse(values[0]);
            id2 = int.Parse(values[1]);
        }
    }
}
