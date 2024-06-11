using Projekat.Controller;
using Projekat.Model;
using Projekat.Model.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for StudentCreate.xaml
    /// </summary>
    public partial class PredmetCreate : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly PredmetController _controller;

        private string sifra;

        public string Sifra
        {
            get
            {
                return sifra;
            }
            set
            {
                if (sifra != value)
                {
                    sifra = value;
                    OnPropertyChanged("Sifra");
                }
            }
        }

        private string naziv;

        public string Naziv
        {
            get
            {
                return naziv;
            }
            set
            {
                if (naziv != value)
                {
                    naziv = value;
                    OnPropertyChanged("Naziv");
                }
            }
        }

        private string semestar;

        public string Semestar
        {
            get
            {
                return semestar;
            }
            set
            {
                if (semestar != value)
                {
                    semestar = value;
                    OnPropertyChanged("Semestar");
                }
            }
        }

        private string godinaStudija;

        public string GodinaStudija
        {
            get
            {
                return godinaStudija;
            }
            set
            {
                if (godinaStudija != value)
                {
                    godinaStudija = value;
                    OnPropertyChanged("GodinaStudija");
                }
            }
        }

        private string espb;

        public string Espb
        {
            get
            {
                return espb;
            }
            set
            {
                if (espb != value)
                {
                    espb = value;
                    OnPropertyChanged("GodinaUpisa");
                }
            }
        }
        public string Error => null;

        private Regex _EspbRegex = new Regex(@"^[1-9]{1}[0-9]{0,1}$");

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Sifra")
                {
                    if (string.IsNullOrEmpty(Sifra))
                        return "Šifra je obavezna";
                }
                else if (columnName == "Naziv")
                {
                    if (string.IsNullOrEmpty(Naziv))
                        return "Naziv je obavezan";
                }
                else if (columnName == "Semestar")
                {
                    if (string.IsNullOrEmpty(Semestar))
                        return "Mora biti izabrana jedna opcija";
                }
                else if (columnName == "GodinaStudija")
                {
                    if (string.IsNullOrEmpty(GodinaStudija))
                        return "Mora biti izabrana jedna opcija";
                }
                else if (columnName == "Espb")
                {
                    if (string.IsNullOrEmpty(Espb))
                        return "Broj ESPB je obavezan";

                    if (Espb[0] == '0')
                        return "Prva cifra ne sme biti 0";

                    Match match = _EspbRegex.Match(Espb);
                    if (!match.Success)
                        return "Broj ESPB mora sadrzati samo 1-2 cifre";

                    if (Convert.ToInt32(Espb) <= 0)
                        return "Broj ESPB mora biti veći od nula";
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Sifra", "Naziv", "Semestar", "GodinaStudija", "Espb" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }


        public PredmetCreate(PredmetController controller)
        {
            InitializeComponent();
            DataContext = this;
            _controller = controller;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            _controller.Create(Sifra, Naziv, Semestar, GodinaStudija, Espb);
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsValid)
                ButtonPotvrdi.IsEnabled = true;
            else
                ButtonPotvrdi.IsEnabled = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsValid)
                ButtonPotvrdi.IsEnabled = true;
            else
                ButtonPotvrdi.IsEnabled = false;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsValid)
                ButtonPotvrdi.IsEnabled = true;
            else
                ButtonPotvrdi.IsEnabled = false;
        }
    }
}
