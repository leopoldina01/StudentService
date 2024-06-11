using Projekat.Controller;
using Projekat.Model;
using Projekat.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for DodavanjePredmeta.xaml
    /// </summary>
    public partial class DodavanjePredmeta : Window, IObserver
    {
        public Student SelectedStudent { get; set; }
        public ObservableCollection<Predmet> Predmeti { get; set; }

        private PredmetController _predmetController;
        private StudentController _studentController;

        public Predmet SelectedPredmet { get; set; }
        public DodavanjePredmeta(Student selectedStudent, PredmetController predmetController, StudentController studentController)
        {
            InitializeComponent();
            DataContext = this;

            SelectedStudent = selectedStudent;
            _studentController = studentController;
            _studentController.Subscribe(this);
            _predmetController = predmetController;
            _predmetController.Subscribe(this);

            Predmeti = new ObservableCollection<Predmet>();
            LoadPredmeti();
        }

        private void LoadPredmeti()
        {
            Predmeti.Clear();
            foreach (Predmet predmet in _predmetController.GetAllPredmeti())
            {
                if (SelectedStudent.godinaStudija < predmet.godinaStudija) continue;

                if (SelectedStudent.nepolozeniPredmeti.Find(p => p.id == predmet.id) != null) continue;

                if (SelectedStudent.polozeniPredmeti.Find(o => o.predmetId == predmet.id) != null) continue;

                Predmeti.Add(predmet);
            }
        }

        private void Button_Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPredmet != null)
            {
                _studentController.AddNepolozeniPredmet(SelectedStudent, SelectedPredmet);
            }
            else
            {
                MessageBox.Show("Odaberite predmet koji želite da dodate", "Nije odabran nijedan predmet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Update()
        {
            LoadPredmeti();
        }
    }
}
