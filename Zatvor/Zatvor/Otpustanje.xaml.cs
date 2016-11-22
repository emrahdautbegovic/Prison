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
    /// Interaction logic for Otpustanje.xaml
    /// </summary>
    public partial class Otpustanje : Window {
        private OsobaKontrola o;
        private MainWindow mainWindow;

        public Otpustanje() {
            InitializeComponent();
        }

        public Otpustanje(OsobaKontrola o, MainWindow mainWindow) {
            InitializeComponent();
            this.o = o;
            this.mainWindow = mainWindow;
            Zatvorenik z = o.dajOsobu() as Zatvorenik;
            slika.Source = z.Slika.Source;
            ime.Text = z.Ime;
            prezime.Text = z.Prezime;
        }

        private void btnPotvrdi_Click(object sender, RoutedEventArgs e) {
            if ((bool)rbtnUslovno.IsChecked != (bool)rbtnPuno.IsChecked) {
                try {
                    DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                    d.kreirajKonekciju();
                    DAL_DAO.DAL.ArhivDAO ad = d.getDAO.getArhivDAO();
                    DAL_DAO.DAL.ZatvorenikDAO zd = d.getDAO.getZatvorenikDAO();
                    Arhiv a = new Arhiv(o.dajOsobu() as Zatvorenik, DateTime.Now, (bool)rbtnPuno.IsChecked ? new TextRange(richNapomena.Document.ContentStart, richNapomena.Document.ContentEnd).Text : "Uslovno");
                    ad.create(a);
                    zd.delete(o.dajOsobu() as Zatvorenik);
                    d.terminirajKonekciju();
                    if ((bool)rbtnPuno.IsChecked) {
                        mainWindow.inicijalizirajArhiv();
                        System.Windows.Forms.MessageBox.Show("Zatvorenik je otpušten, prebaćen u arhiv!");
                    } else {
                        mainWindow.inicijalizirajUslovne();
                         System.Windows.Forms.MessageBox.Show("Zatvorenik je pušten uslovno!"); 
                    }
                    mainWindow.inicijalizirajZatvorenike();
                    return;              
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            } else System.Windows.Forms.MessageBox.Show("Niste odabrali vrstu puštanja!");
        }

        private void btnKraj_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void rbtnUslovno_Checked(object sender, RoutedEventArgs e) {
            if ((bool)rbtnUslovno.IsChecked) {
                richNapomena.AppendText("Uslovno");
                richNapomena.IsEnabled = false;
            }
        }

        private void rbtnPuno_Checked(object sender, RoutedEventArgs e) {
            if ((bool)rbtnPuno.IsChecked) {
                richNapomena.IsEnabled = true;
                richNapomena.Document.Blocks.Clear();
            }
        }
    }
}
