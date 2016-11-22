using Microsoft.Win32;
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

namespace Zatvor {
    /// <summary>
    /// Interaction logic for UnosUposlenika.xaml
    /// </summary>
    public partial class UnosUposlenika : Window {
        private OsobaKontrola o;
        private MainWindow mainWindow;                
        bool zaUredjivanje = false;
        public UnosUposlenika(MainWindow m) {
            InitializeComponent();
            mainWindow = m;
        }

        public UnosUposlenika(OsobaKontrola o, MainWindow mainWindow) {
            InitializeComponent();
            this.zaUredjivanje = true;
            this.Title = "Uređivanje uposlenika";
            this.btnUnos.Content = "Promijeni";
            InitializeComponent();
            Osoba osoba = o.dajOsobu();
            this.o = o;
            this.mainWindow = mainWindow;
            txtName.Text = osoba.Ime;
            txtPrezime.Text = osoba.Prezime;
            txtAdresa.Text = osoba.Adresa;
            cboxSpol.SelectedIndex = Convert.ToInt32(osoba.Spol);
            dateRodjenje.SelectedDate = (DateTime?)osoba.DatumRodjenja;
            txtId.Value = osoba.Id;
            txtId.IsEnabled = false;
            slikaUposlenika.Source = osoba.Slika.Source;

            if (osoba.GetType() == typeof(Upravnik)) {
                cboxPosao.SelectedIndex = 1;
                richNapomene.AppendText((osoba as Upravnik).Napomene);
                dateZaposlenje.SelectedDate = (DateTime?)(osoba as Upravnik).DatumZaposlenja;
            } else if (osoba.GetType() == typeof(UpravnikOdjela)) {
                cboxPosao.SelectedIndex = 2;
                richNapomene.AppendText((osoba as UpravnikOdjela).Napomene);
                dateZaposlenje.SelectedDate = (DateTime?)(osoba as UpravnikOdjela).DatumZaposlenja;
                cboxBlok.SelectedIndex = Convert.ToInt32((osoba as UpravnikOdjela).Blok);
            } else {
                cboxPosao.SelectedIndex = 0;
                richNapomene.AppendText((osoba as Cuvar).Napomene);
                dateZaposlenje.SelectedDate = (DateTime?)(osoba as Cuvar).DatumZaposlenja;
                cboxBlok.SelectedIndex = Convert.ToInt32((osoba as Cuvar).Blok);
                txtZaduzenje.Text = (osoba as Cuvar).Zaduzenje;
            }
        }

        private void btnUnos_Click(object sender, RoutedEventArgs e) {
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                if (cboxPosao.SelectedIndex == 0) {
                    DAL_DAO.DAL.CuvarDAO cd = d.getDAO.getCuvarDAO();
                    Cuvar c = new Cuvar(txtName.Text, txtPrezime.Text, txtAdresa.Text, (DateTime)dateRodjenje.SelectedDate, Convert.ToInt32(txtId.Value), (Spol)cboxSpol.SelectedIndex,
                       new TextRange(richNapomene.Document.ContentStart, richNapomene.Document.ContentEnd).Text, (Blok)cboxBlok.SelectedIndex, (DateTime)dateZaposlenje.SelectedDate, new List<Prisustvo>(), txtZaduzenje.Text, slikaUposlenika);
                    if (!zaUredjivanje) cd.create(c);                    
                    else cd.update(c);
                } else if (cboxPosao.SelectedIndex == 1) {
                    DAL_DAO.DAL.UpravnikDAO ud = d.getDAO.getUpravnikDAO();
                    Upravnik u = new Upravnik(txtName.Text, txtPrezime.Text, txtAdresa.Text, (DateTime)dateRodjenje.SelectedDate, Convert.ToInt32(txtId.Value),
                        (Spol)cboxSpol.SelectedIndex, new TextRange(richNapomene.Document.ContentStart, richNapomene.Document.ContentEnd).Text, (DateTime)dateZaposlenje.SelectedDate, new List<Prisustvo>(), slikaUposlenika);
                    if (!zaUredjivanje) ud.create(u);
                    else ud.update(u);
                } else if (cboxPosao.SelectedIndex == 2) {
                    DAL_DAO.DAL.UpravnikOdjelaDAO uod = d.getDAO.getUpravnikOdjelaDAO();
                    UpravnikOdjela uo = new UpravnikOdjela(txtName.Text, txtPrezime.Text, txtAdresa.Text, (DateTime)dateRodjenje.SelectedDate, Convert.ToInt32(txtId.Value), (Spol)cboxSpol.SelectedIndex,
                       new TextRange(richNapomene.Document.ContentStart, richNapomene.Document.ContentEnd).Text, (DateTime)dateZaposlenje.SelectedDate, new List<Prisustvo>(), (Blok)cboxBlok.SelectedIndex, slikaUposlenika);
                    if (!zaUredjivanje) uod.create(uo);
                    else uod.update(uo);
                }
                d.terminirajKonekciju();
                if (!zaUredjivanje) MessageBox.Show("Uposlenik unešen!");
                else System.Windows.Forms.MessageBox.Show("Podaci izmjenjeni!");
                mainWindow.inicijalizirajUposlenike();
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void cboxPosao_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cboxPosao.SelectedIndex == 1) cboxBlok.IsEnabled = false;
            else cboxBlok.IsEnabled = true;
        }

        private void btnSlikaZatvorenika_Click(object sender, RoutedEventArgs e) {
            try {
                OpenFileDialog o = new OpenFileDialog();
                bool? ok = o.ShowDialog();
                if (ok ?? true) {
                    slikaUposlenika.Source = new BitmapImage(new Uri(o.FileName));
                }
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
