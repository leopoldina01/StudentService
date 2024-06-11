using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projekat.Manager.Serialization;
using Projekat.Model;

namespace Projekat.Manager
{
    class StudentManager
    {
        public ParentManager parentManager { get; set; }

        private readonly string fileName = "studenti.txt";

        public StudentManager(ParentManager parentManager)
        {
            this.parentManager = parentManager;
            loadStudents();
        }

        private void loadStudents()
        {
            List<ManyToManySerialization> studentiOcene = new List<ManyToManySerialization>();
            List<ManyToManySerialization> studentiPredmeti = new List<ManyToManySerialization>();

            Serializer<ManyToManySerialization> studentiOceneSerializer = new Serializer<ManyToManySerialization>();
            Serializer<ManyToManySerialization> studentiPredmetiSerializer = new Serializer<ManyToManySerialization>();

            studentiOcene = studentiOceneSerializer.FromCSV("StudentiOcene.txt");
            studentiPredmeti = studentiPredmetiSerializer.FromCSV("StudentiPredmeti.txt");

            foreach(Student student in parentManager.studenti)
            {
                Adresa adresa = parentManager.adrese.Find(a => a.Id == student.adresaId);

                if (adresa == null) continue;
                student.adresa = adresa;
            }

            foreach(ManyToManySerialization studentOcena in studentiOcene)
            {
                Student student = parentManager.studenti.Find(s => s.id == studentOcena.id1);
                Ocena ocena = parentManager.ocene.Find(o => o.Id == studentOcena.id2);
                if (ocena == null || student == null) continue;
                student.polozeniPredmeti.Add(ocena);
            }

            foreach(ManyToManySerialization studentPredmet in studentiPredmeti)
            {
                Student student = parentManager.studenti.Find(s => s.id == studentPredmet.id1);
                Predmet predmet = parentManager.predmeti.Find(p => p.id == studentPredmet.id2);
                if (student == null || predmet == null) continue;
                student.nepolozeniPredmeti.Add(predmet);
            }
        }

        private void saveStudents()
        {
            parentManager.studentSerializer.ToCSV(fileName, parentManager.studenti);
            List<ManyToManySerialization> studentiOcene = new List<ManyToManySerialization>();
            foreach (Student student in parentManager.studenti)
            {
                foreach (Ocena ocena in student.polozeniPredmeti)
                {
                    ManyToManySerialization temp = new ManyToManySerialization();
                    temp.id1 = student.id;
                    temp.id2 = ocena.Id;
                    studentiOcene.Add(temp);
                }
            }
            Serializer<ManyToManySerialization> studentiOceneSerializer = new Serializer<ManyToManySerialization>();
            studentiOceneSerializer.ToCSV("studentiOcene.txt", studentiOcene);

            List<ManyToManySerialization> studentiPredmeti = new List<ManyToManySerialization>();
            foreach (Student student in parentManager.studenti)
            {
                foreach (Predmet predmet in student.nepolozeniPredmeti)
                {
                    ManyToManySerialization temp = new ManyToManySerialization();
                    temp.id1 = student.id;
                    temp.id2 = predmet.id;
                    studentiPredmeti.Add(temp);
                }
            }
            Serializer<ManyToManySerialization> studentiPredmetiSerializer = new Serializer<ManyToManySerialization>();
            studentiPredmetiSerializer.ToCSV("studentiPredmeti.txt", studentiPredmeti);

        }

        private int generateId()
        {
            if (parentManager.studenti.Count == 0) return 0;
            return parentManager.studenti[parentManager.studenti.Count - 1].id + 1;
        }

        public Student addStudent(Student student)
        {
            student.id = generateId();
            parentManager.studenti.Add(student);
            saveStudents();
            return student;
        }

        public Student updateStudent(Student student)
        {
            Student oldStudent = GetStudentById(student.id);
            if (oldStudent == null) return null;

            oldStudent.prezime = student.prezime;
            oldStudent.ime = student.ime;
            oldStudent.datumRodjenja = student.datumRodjenja;
            oldStudent.adresa = student.adresa;
            oldStudent.adresaId = student.adresaId;
            oldStudent.brTelefona = student.brTelefona;
            oldStudent.email = student.email;
            oldStudent.brIndeksa = student.brIndeksa;
            oldStudent.godinaUpisa = student.godinaUpisa;
            oldStudent.godinaStudija = student.godinaStudija;
            oldStudent.status = student.status;
            oldStudent.prosecnaOcena = student.prosecnaOcena;
            oldStudent.polozeniPredmeti = student.polozeniPredmeti;
            oldStudent.nepolozeniPredmeti = student.nepolozeniPredmeti;

            saveStudents();
            return oldStudent;
        }

        public Student RemoveStudent(int id)
        {
            Student student = GetStudentById(id);
            if (student == null) return null;

            parentManager.studenti.Remove(student);
            saveStudents();
            return student;
        }

        public Student GetStudentById(int id)
        {
            return parentManager.studenti.Find(s => s.id == id);
        }

        public List<Student> GetAllStudents()
        {
            return parentManager.studenti;
        }
    }
}
