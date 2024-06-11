using Projekat.Manager.Serialization;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projekat.Storage
{
    class ProfesorStorage
    {
        private readonly string StoragePathProfesori = string.Format("..{0}..{0}..{0}Data{0}profesori.csv", Path.DirectorySeparatorChar);
        private readonly Serializer<Profesor> _serializer;

        public ProfesorStorage()
        {
            _serializer = new Serializer<Profesor>();
        }

        public List<Profesor> Load()
        {
            return _serializer.FromCSV(StoragePathProfesori);
        }

        public void Save(List<Profesor> profesori)
        {
            _serializer.ToCSV(StoragePathProfesori, profesori);
        }
    }
}
