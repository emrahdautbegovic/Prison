using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.IO;
using Microsoft.Win32;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Zatvor {
    /// <summary>
    /// Interaction logic for UnosZatvorenika.xaml
    /// </summary>
    public partial class UnosZatvorenika : Window {
        private Zatvorenik zatvorenik;
        bool zaPovratak = false;
        bool zaUređivanje = false;
        private MainWindow mainWindow;
        public UnosZatvorenika() {
            InitializeComponent();
        }

        public UnosZatvorenika(MainWindow m) {
            InitializeComponent();
            mainWindow = m;
        }
        
        public UnosZatvorenika(Zatvorenik z, MainWindow mainWindow, bool zaPovratak = false) {
            
            if(zaPovratak) this.zaPovratak = zaPovratak;
            else zaUređivanje = true;
            InitializeComponent();
            this.zatvorenik = z;
            this.mainWindow = mainWindow;
            uredi(z);
        }
        private void uredi(Zatvorenik z) {
            txtIme.Text = z.Ime;
            txtPrezime.Text = z.Prezime;
            txtAdresa.Text = z.Adresa;
            txtId.Value = z.Id;
            txtCelija.Value = z.BrojCelije;
            dtmDolazak.SelectedDate = (DateTime?)z.DatumDolaska;
            dtmOdlazak.SelectedDate = (DateTime?)z.DatumOdlaska;
            dtmRodjenje.SelectedDate = (DateTime?)z.DatumRodjenja;
            comboBoxBlok.SelectedIndex = Convert.ToInt32(z.Blok);
            rtxtNapomena.AppendText(z.Napomene);
            slikaZatvorenika.Source = z.Slika.Source;
            btnUnos.Content = "Promijeni";
            this.Title = "Uređivanje zatvorenika!";
            txtId.IsEnabled = false;
        }
        private void btnUnos_Click(object sender, RoutedEventArgs e) {
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.ZatvorenikDAO zd = d.getDAO.getZatvorenikDAO();
                Zatvorenik z = new Zatvorenik(txtIme.Text, txtPrezime.Text, txtAdresa.Text, (DateTime)dtmRodjenje.SelectedDate, Convert.ToInt32(txtId.Value),
                    Spol.Musko, new TextRange(rtxtNapomena.Document.ContentStart, rtxtNapomena.Document.ContentEnd).Text, (Blok)Enum.Parse(typeof(Blok), comboBoxBlok.Text.Replace(" ", string.Empty), true),
                    Convert.ToInt32(txtCelija.Value), (DateTime)dtmDolazak.SelectedDate, (DateTime)dtmOdlazak.SelectedDate, slikaZatvorenika);
                if (zaUređivanje) {
                    zd.update(z);
                    System.Windows.Forms.MessageBox.Show("Podaci su izmijenjeni!");
                } else if (zaPovratak) {
                    DAL_DAO.DAL.ArhivDAO ad = d.getDAO.getArhivDAO();
                    ad.delete(new Arhiv(z, DateTime.Now, ""));
                    zd.create(z);
                    System.Windows.Forms.MessageBox.Show("Osuđenik je vraćen u zatvor!");
                }else {
                    zd.create(z);
                    System.Windows.Forms.MessageBox.Show("Zatvorenik unešen!");
                }
                mainWindow.inicijalizirajZatvorenike();
                d.terminirajKonekciju();
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnSlikaZatvorenik_Click(object sender, RoutedEventArgs e) {
            try {
                OpenFileDialog o = new OpenFileDialog();
                bool? ok = o.ShowDialog();
                if (ok ?? true) slikaZatvorenika.Source = new BitmapImage(new Uri(o.FileName));
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}