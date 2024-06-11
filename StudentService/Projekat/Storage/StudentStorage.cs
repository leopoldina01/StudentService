using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projekat.Model;
using Projekat.Manager.Serialization;
using System.IO;

namespace Projekat.Storage
{
    class StudentStorage
    {
        private readonly string StoragePath = string.Format("..{0}..{0}..{0}Data{0}students.csv", Path.DirectorySeparatorChar);
        private readonly string StoragePathPolozeni = string.Format("..{0}..{0}..{0}Data{0}studentiPredmeti.csv", Path.DirectorySeparatorChar);


        private readonly Serializer<Student> _serializer;
        private readonly Serializer<ManyToManySerialization> _serializerManyToMany;


        public StudentStorage()
        {
            _serializer = new Serializer<Student>();
            _serializerManyToMany = new Serializer<ManyToManySerialization>();
        }

        public List<Student> Load()
        {
            return _serializer.FromCSV(StoragePath);
        }

        public List<ManyToManySerialization> LoadStudentiPredmeti()
        {
            return _serializerManyToMany.FromCSV(StoragePathPolozeni);
        }

        public void Save(List<Student> students)
        {
            _serializer.ToCSV(StoragePath, students);
        }
    }
}
