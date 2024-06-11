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
    /// Interaction logic for OdabirProfesora.xaml
    /// </summary>
    public partial class OdabirProfesora : Window, IObserver, INotifyPropertyChanged
    {
        private readonly ProfesorController _profesorController;
        private readonly PredmetController _predmetController;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Profesor> Profesori { get; set; }
        public Profesor SelectedProfesor { get; set; }
        private Predmet SelectedPredmet { get; set; }

        public OdabirProfesora(ProfesorController profesorController, Predmet selectedPredmet, PredmetController predmetController)
        {
            InitializeComponent();
            DataContext = this;

            SelectedPredmet = selectedPredmet;
            _profesorController = profesorController;
            _predmetController = predmetController;
            _predmetController.Subscribe(this);


            Profesori = new ObservableCollection<Profesor>(_profesorController.GetAllProfesors());
            UpdateProfesorList();
        }

        private void UpdateProfesorList()
        {
            Profesori.Clear();
            foreach (var profesor in _profesorController.GetAllProfesors())
            {
                Profesori.Add(profesor);
            }
        }

        public void Update()
        {
            UpdateProfesorList();
        }

        private void ButtonOdustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(SelectedProfesor.godineStaza.ToString());
            if (SelectedProfesor != null)
            {

                //MessageBox.Show(SelectedProfesor.ime);
                //_profesorController.DodajPredmet(SelectedProfesor, SelectedPredmet);
                //posle ovoga selectedprofesor postaje null
                //MessageBox.Show(SelectedProfesor.prezime);
                _predmetController.DodajProfesora(SelectedPredmet, SelectedProfesor, _profesorController);
                //MessageBox.Show(SelectedPredmet.profesor.ime);
                //_predmetController.LoadComplexData(_profesorController);

                this.Close();
            }
            else
            {
                MessageBox.Show("Odaberite profesora kojeg želite da dodate", "Nije odabran nijedan profesor", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }
    }
}
