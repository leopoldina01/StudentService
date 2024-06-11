using Projekat.Controller;
using Projekat.Model;
using Projekat.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DodavanjePredmetaProfesoru.xaml
    /// </summary>
    public partial class DodavanjePredmetaProfesoru : Window, IObserver
    {
        public Profesor SelectedProfesor { get; set; }
        public ObservableCollection<Predmet> Predmeti { get; set; }

        private PredmetController _predmetController;
        private ProfesorController _profesorController;

        public Predmet SelectedPredmet { get; set; }

        public DodavanjePredmetaProfesoru(Profesor selectedProfesor, PredmetController predmetController, ProfesorController profesorController)
        {
            InitializeComponent();
            DataContext = this;

            SelectedProfesor = selectedProfesor;
            _profesorController = profesorController;
            _profesorController.Subscribe(this);
            _predmetController = predmetController;

            Predmeti = new ObservableCollection<Predmet>();
            LoadPredmeti();
        }

        private void LoadPredmeti()
        {
            Predmeti.Clear();
            foreach (Predmet predmet in _predmetController.GetAllPredmeti())
            {
                if (predmet.profesor != null) continue;
                Predmeti.Add(predmet);
            }
        }

        private void Button_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPredmet != null)
            {
                _profesorController.DodajPredmet(SelectedProfesor, SelectedPredmet);
                this.Close();
            }
            else
            {
                MessageBox.Show("Odaberite predmet koji želite da dodate", "Nije odabran nijedan predmet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Update()
        {
            LoadPredmeti();
        }
    }
}
