using Projekat.Manager.Serialization;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projekat.Storage
{
    class KatedraStorage
    {
        private readonly string StoragePath = string.Format("..{0}..{0}..{0}Data{0}katedre.csv", Path.DirectorySeparatorChar);
        private readonly Serializer<Katedra> _serializer;

        public KatedraStorage()
        {
            _serializer = new Serializer<Katedra>();
        }

        public List<Katedra> Load()
        {
            return _serializer.FromCSV(StoragePath);
        }

        public void Save(List<Katedra> katedre)
        {
            _serializer.ToCSV(StoragePath, katedre);
        }
    }
}
