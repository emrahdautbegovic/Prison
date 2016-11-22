using Microsoft.Expression.Encoder.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WebcamControl;
using System.Drawing.Imaging;

namespace Zatvor {
    public partial class MainWindow {
        internal void inicijalizirajSve() {
            inicijalizirajZatvorenike();
            inicijalizirajCuvare();
            inicijalizirajUpravnika();
            inicijalizirajUpravnikeOdjela();
            inicijalizirajArhiv();
            inicijalizirajUslovne();
            inicijalizirajUsere();
            inicijalizirajPrisustva();
            inicijalizirajAdmine();
           // inicijalizirajKameru();
        }

        internal void inicijalizirajUsere() {
            PanelUseri.Children.Clear();
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.UserDAO ud = d.getDAO.getUserDAO();
                useri = ud.getAll();
                foreach (User u in useri) {
                    OsobaKontrola o = new OsobaKontrola(u);
                    PanelUseri.Children.Add(o);
                }
                d.terminirajKonekciju();
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Problem kod inicjalizacije zatvorenika: " + ex.Message);
            }
        }


        internal void inicijalizirajUposlenike() {
            inicijalizirajCuvare();
            inicijalizirajUpravnika();
            inicijalizirajUpravnikeOdjela();
        }


        internal void inicijalizirajZatvorenike() {
            try {
                ocistiZatvorenike();
                inicijalizirajArhiv();
                inicijalizirajUslovne();
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.ZatvorenikDAO zd = d.getDAO.getZatvorenikDAO();
                zatvorenici = zd.getAll();
                foreach (Zatvorenik z in zatvorenici) {
                    OsobaKontrola o = new OsobaKontrola(z);
                    switch (z.Blok) {
                        case Blok.BlokA: PanelBlokA.Children.Add(o); break;
                        case Blok.BlokB: PanelBlokB.Children.Add(o); break;
                        case Blok.BlokC: PanelBlokC.Children.Add(o); break;
                        case Blok.Samica: PanelSamica.Children.Add(o); break;
                        case Blok.OdjelZaSmrtneKazne: PanelSmrt.Children.Add(o); break;
                    }
                }
                d.terminirajKonekciju();
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Problem kod inicjalizacije zatvorenika: " + ex.Message);
            }
        }

        internal void inicijalizirajUpravnika() {
            ocistiUpravnika();
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.UpravnikDAO ud = d.getDAO.getUpravnikDAO();
                upravnik = ud.dajUpravnika();
                if (upravnik == null) { d.terminirajKonekciju(); return; }
                imgSlikaUpravnika.Source = upravnik.Slika.Source;
                ime.Text = upravnik.Ime;
                prezime.Text = upravnik.Prezime;
                adresa.Text = upravnik.Adresa;
                datumrodjenja.Text = upravnik.DatumRodjenja.ToShortDateString();
                spol.Text = upravnik.Spol.ToString();
                napomene.Text = upravnik.Napomene;
                d.terminirajKonekciju();
            } catch (Exception ex) {
                System.Windows.MessageBox.Show("Problem kod inicjalizacije upravnika: " + ex.Message);
            }
        }

        internal void inicijalizirajCuvare() {
            PanelCuvari.Children.Clear();
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.CuvarDAO cd = d.getDAO.getCuvarDAO();
                cuvari = cd.getAll();
                foreach (Cuvar c in cuvari) {
                    OsobaKontrola _cuvar = new OsobaKontrola(c);
                    PanelCuvari.Children.Add(_cuvar);
                }
                d.terminirajKonekciju();
            } catch (Exception ex) {
                System.Windows.MessageBox.Show("Problem kod inicjalizacije čuvara: " + ex.Message);
            }

        }

        internal void inicijalizirajUpravnikeOdjela() {
            panelUpravnici.Children.Clear();
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.UpravnikOdjelaDAO uod = d.getDAO.getUpravnikOdjelaDAO();
                upravniciOdjela = uod.getAll();
                foreach (UpravnikOdjela uo in upravniciOdjela) {
                    OsobaKontrola _upravnikOodjela = new OsobaKontrola(uo);
                    panelUpravnici.Children.Add(_upravnikOodjela);
                }
                d.terminirajKonekciju();
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Problem kod inicjalizacije upravnika odjela: " + ex.Message);
            }
        }

        internal void inicijalizirajArhiv() {
            PanelArhiv.Children.Clear();
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.ArhivDAO ad = d.getDAO.getArhivDAO();
                arhivi = ad.getAll();
                foreach (Arhiv ar in arhivi) {
                    OsobaKontrola o = new OsobaKontrola(ar.Zatvorenik);
                    PanelArhiv.Children.Add(o);
                }
                d.terminirajKonekciju();
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Problem kod inicjalizacije arhiva: " + ex.Message);
            }
        }
        internal void inicijalizirajUslovne() {
            PanelUslovno.Children.Clear();
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.ArhivDAO ad = d.getDAO.getArhivDAO();
                arhiviUslovni = ad.dajUslovne();
                foreach (Arhiv ar in arhiviUslovni) {
                    OsobaKontrola o = new OsobaKontrola(ar.Zatvorenik);
                    PanelUslovno.Children.Add(o);
                }
                d.terminirajKonekciju();
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("Problem kod inicjalizacije uslovnih: " + ex.Message);
            }
        }

        internal void inicijalizirajPrisustva() {
            lboxPrisustva.Items.Clear();
            DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
            d.kreirajKonekciju();
            DAL_DAO.DAL.PrisustvoDAO pd = d.getDAO.getPrisustvoDAO();
            prisustva = pd.getAll();            
            foreach (Prisustvo p in prisustva) {
                lboxPrisustva.Items.Add(p);
            }
            d.terminirajKonekciju();
        }

        internal void inicijalizirajAdmine() {
            PanelAdmini.Children.Clear();
            DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
            d.kreirajKonekciju();
            DAL_DAO.DAL.AdminDAO ad = d.getDAO.getAdminDAO();
            admini = ad.getAll();
            if(admini != null)
            foreach (Admin a in admini) {
                PanelAdmini.Children.Add(new OsobaKontrola(a));
            }
            d.terminirajKonekciju();
        }

         private Webcam WebcamCtrl;         
        internal void inicijalizirajKameru() {
            WebcamCtrl = new Webcam();
            WebcamCtrl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            WebcamCtrl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            WebcamCtrl.Visibility = System.Windows.Visibility.Visible;
            WebcamCtrl.Height = 138;
            WebcamCtrl.Width = 173;            
            

            Binding binding_1 = new Binding("SelectedValue");
            binding_1.Source = VideoDevicesComboBox;
            WebcamCtrl.SetBinding(Webcam.VideoDeviceProperty, binding_1);

            /*Binding binding_2 = new Binding("SelectedValue");
            binding_2.Source = AudioDevicesComboBox;
            WebcamCtrl.SetBinding(Webcam.AudioDeviceProperty, binding_2);  */                  
            // Create directory for saving image files
            string imagePath = @"C:\WebcamSnapshots";            
            if (!Directory.Exists(imagePath)) {
                Directory.CreateDirectory(imagePath);
            }

            // Set some properties of the Webcam control            
            WebcamCtrl.ImageDirectory = imagePath;
            WebcamCtrl.FrameRate = 30;
            WebcamCtrl.FrameSize = new System.Drawing.Size(640, 480);

            // Find available a/v devices
            var vidDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
            //var audDevices = EncoderDevices.FindDevices(EncoderDeviceType.Audio);
            VideoDevicesComboBox.ItemsSource = vidDevices;
            //AudioDevicesComboBox.ItemsSource = audDevices;
            VideoDevicesComboBox.SelectedIndex = 0;
            //AudioDevicesComboBox.SelectedIndex = 0;
            AudioDevicesComboBox.Visibility = System.Windows.Visibility.Hidden;

            gridCam.Children.Add(WebcamCtrl);
            
        }
    }
}
