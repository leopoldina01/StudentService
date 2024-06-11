using Projekat.Manager.Serialization;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projekat.Storage
{
    class AdresaStorage
    {
        private readonly string StoragePath = string.Format("..{0}..{0}..{0}Data{0}adrese.csv", Path.DirectorySeparatorChar);

        private readonly Serializer<Adresa> _serializer;

        public AdresaStorage()
        {
            _serializer = new Serializer<Adresa>();
        }

        public List<Adresa> Load()
        {
            return _serializer.FromCSV(StoragePath);
        }

        public void Save(List<Adresa> adrese)
        {
            _serializer.ToCSV(StoragePath, adrese);
        }
    }
}
