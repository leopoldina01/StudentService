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
    /// Interaction logic for DodavanjeSefa.xaml
    /// </summary>
    public partial class DodavanjeSefa : Window
    {
        public Katedra SelectedKatedra { get; set; }

        public Profesor SelectedProfesor { get; set; }
        public Profesor stariSef { get; set; }

        public ObservableCollection<Profesor> Profesori { get; set; }

        private KatedraController _katedraController;
        private ProfesorController _profesorController;

        public DodavanjeSefa(Katedra selectedKatedra, ProfesorController profesorController, KatedraController katedraController)
        {
            InitializeComponent();
            DataContext = this;

            SelectedKatedra = selectedKatedra;
            _katedraController = katedraController;
            _profesorController = profesorController;

            stariSef = profesorController.GetAllProfesors().Find(p => p.Id == SelectedKatedra.sefId);

            Profesori = new ObservableCollection<Profesor>();
            LoadProfesori();
        }

        private void LoadProfesori()
        {
            Profesori.Clear();
            foreach(Profesor profesor in _profesorController.GetAllProfesors())
            {
                if (SelectedKatedra.sefId == profesor.Id) continue;

                if (profesor.zvanje.ToLower() != "redovni profesor" && profesor.zvanje.ToLower() != "vanredni profesor") continue;

                if (profesor.godineStaza < 5) continue;

                Profesori.Add(profesor);
            }
        }

        private void Button_Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Postavi_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProfesor != null)
            {
                _katedraController.SetSefKatedre(SelectedKatedra, SelectedProfesor, _profesorController, stariSef);
                this.Close();
            }
            else
            {
                MessageBox.Show("Odaberite profesora kojeg želite da postavite za šefa katedre", "Nije odabran nijedan profesor", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
