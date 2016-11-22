using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.IO;


namespace Zatvor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        private List<Zatvorenik> zatvorenici;
        private Upravnik upravnik;
        private List<Cuvar> cuvari;
        private List<UpravnikOdjela> upravniciOdjela;
        private List<Arhiv> arhivi;
        private List<Arhiv> arhiviUslovni;
        private List<Prisustvo> prisustva;
        private List<User> useri;
        private List<Admin> admini;
        private Admin admin = null;
        private User user = null;
        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            //DragMove();
        }


        private void zabPass_Click(object sender, RoutedEventArgs e) {
            ZaboravioPass z = new ZaboravioPass();
            z.Show();
        }

        private void prikazZaGlavnogAdmina() {
            logo.Width = 725;
            zabPass.Visibility = Visibility.Hidden;
            TabControlGalvni.Visibility = Visibility.Visible;
            btnLogOut.Visibility = Visibility.Visible;
            Grb.Visibility = Visibility.Hidden;
        }

        private void prikazZaAdmina() {
            prikazZaGlavnogAdmina();
            btnDodajAdmina.Visibility = Visibility.Hidden;
            btnUrediAdmina.Visibility = Visibility.Hidden;
            btnBrisiAdmina.Visibility = Visibility.Hidden;
            btnDodajKor.Visibility = Visibility.Hidden;
            btnBrisiKor.Visibility = Visibility.Hidden;
            btnBrisiPrisustva.Visibility = Visibility.Hidden;
        }

        private void prikazZaUsera() {
            logo.Width = 725;
            zabPass.Visibility = Visibility.Hidden;
            TabControlGalvni.Visibility = Visibility.Visible;
            btnLogOut.Visibility = Visibility.Visible;
            Grb.Visibility = Visibility.Hidden;
            btnDodajUposl.Visibility = Visibility.Hidden;
            btnEdit.Visibility = Visibility.Hidden;
            btnOtpustanje.Visibility = Visibility.Hidden;
            buttonUnosZatvorenika.Visibility = Visibility.Hidden;
            tabKorisnici.Visibility = Visibility.Hidden;
            tabPrisustva.Visibility = Visibility.Hidden;
            btnUrediUposlenika.Visibility = Visibility.Hidden;
            btnOtpustiUposlenika.Visibility = Visibility.Hidden;
            tabIzvjestaj.Visibility = Visibility.Hidden;
        }

        private void potvrdaButton_Click(object sender, RoutedEventArgs e) {
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.UserDAO ud = d.getDAO.getUserDAO();
                user = ud.dajUsera(txtUser.Text, txtPass.Password.ToString());
                if (user != null) { imgLogovani.Source = ud.dajSlikuUseraPoId(user.Id).Source; prikazZaUsera(); 
                } else {
                    DAL_DAO.DAL.AdminDAO ad = d.getDAO.getAdminDAO();
                    admin = ad.dajAdmina(txtUser.Text, txtPass.Password.ToString());
                    if (admin != null) {
                        if (admin.Tip == TipAdmina.Glavni) {
                            imgLogovani.Source = admin.Slika.Source; prikazZaGlavnogAdmina();
                        } else { imgLogovani.Source = admin.Slika.Source; prikazZaAdmina(); }
                    } else {
                        MessageBox.Show("Nepostojeći korisnik !", "Problem kod verifikacije");
                        d.terminirajKonekciju();
                        return;
                    }
                }
                d.terminirajKonekciju();                             
                inicijalizirajSve();
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            prikazZaGlavnogAdmina();
            inicijalizirajSve();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void buttonUnosZatvorenika_Click(object sender, RoutedEventArgs e) {
            UnosZatvorenika u = new UnosZatvorenika(this);
            u.ShowDialog();
        }

        private void btnDodajUposl_Click(object sender, RoutedEventArgs e) {
            UnosUposlenika u = new UnosUposlenika(this);
            u.Show();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e) {
            logo.Width = 380;
            TabControlGalvni.Visibility = Visibility.Hidden;
            zabPass.Visibility = Visibility.Visible;
            btnLogOut.Visibility = Visibility.Hidden;
            Grb.Visibility = Visibility.Visible;
            txtUser.Clear();
            txtPass.Clear();            
            WebcamCtrl = null;
            imgLogovani.Source = null;
            TabControlGalvni.SelectedIndex = 0;
            ocistiPanele();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e) {
            TabControlBlokovi.Height = 200;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e) {
            TabControlBlokovi.Height = 339;
        }

        private void Expander_Collapsed_5(object sender, RoutedEventArgs e) {
            GroupBoxCuvari.Height = 305;
        }

        private void Expander_Expanded_5(object sender, RoutedEventArgs e) {
            GroupBoxCuvari.Height = 177;
        }

        private OsobaKontrola dajKliknutuKontrolu(WrapPanel w) {
            foreach (Control c in w.Children.OfType<OsobaKontrola>()) {
                OsobaKontrola o = c as OsobaKontrola;
                if (o.Klik) return o;
            }
            return null;
        }

        public OsobaKontrola dajSelektovanuKontrolu(WrapPanel w) {
            foreach (Control c in w.Children.OfType<OsobaKontrola>()) {
                OsobaKontrola o = c as OsobaKontrola;
                if ((o.Klik == false && o.BorderBrush == Brushes.Black)) return o;
            }
            return null;
        }

        private void regulisiPanel(WrapPanel w) {
            foreach (Control c in w.Children.OfType<OsobaKontrola>()) {
                OsobaKontrola o = c as OsobaKontrola;
                if ((o.Klik == false && o.BorderBrush == Brushes.Black)) {
                    o.BorderBrush = Brushes.Transparent;
                }
            }
        }
        private void PanelBlokA_MouseDown(object sender, MouseButtonEventArgs e) {
            regulisiPanel(sender as WrapPanel);
            OsobaKontrola o = dajKliknutuKontrolu(sender as WrapPanel);
            if (o == null) return;
            if (o.Klik) {
                Zatvorenik z = o.dajOsobu() as Zatvorenik;
                txtImeA.Text = z.Ime;
                txtAdresaA.Text = z.Adresa;
                txtPrezimeA.Text = z.Prezime;
                txtBlokA.Text = z.Blok.ToString();
                txtBrCelijeA.Text = z.BrojCelije.ToString();
                txtDatumDolaskaA.Text = z.DatumDolaska.ToShortDateString();
                txtDatumIzlaskaA.Text = z.DatumOdlaska.ToShortDateString();
                txtDatumRodjenjaA.Text = z.DatumRodjenja.ToShortDateString();
                txtNapomeneA.Text = z.Napomene;
                txtIDA.Text = z.Id.ToString();
                SlikaPodaci1.Source = z.Slika.Source;
                o.Klik = false;
            }
        }

        private void PanelBlokB_MouseDown(object sender, MouseButtonEventArgs e) {
            PanelBlokA_MouseDown(sender, e);
        }

        private void PanelBlokC_MouseDown(object sender, MouseButtonEventArgs e) {
            PanelBlokA_MouseDown(sender, e);
        }

        private void PanelSamica_MouseDown(object sender, MouseButtonEventArgs e) {
            PanelBlokA_MouseDown(sender, e);
        }

        private void PanelSmrt_MouseDown(object sender, MouseButtonEventArgs e) {
            PanelBlokA_MouseDown(sender, e);
        }

        private void panelUpravnici_MouseDown(object sender, MouseButtonEventArgs e) {
            regulisiPanel(sender as WrapPanel);
            OsobaKontrola o = dajKliknutuKontrolu(sender as WrapPanel);
            if (o == null) return;
            if (o.Klik) {
                UpravnikOdjela uo = o.dajOsobu() as UpravnikOdjela;
                txtIme.Text = uo.Ime;
                txtPrezime.Text = uo.Prezime;
                txtAdresa.Text = uo.Adresa;
                txtID.Text = uo.Id.ToString();
                txtDatumRodjenjaUO.Text = uo.DatumRodjenja.ToShortDateString();
                txtBlok.Text = uo.Blok.ToString();
                txtNapomene.Text = uo.Napomene;
                txtDatumZaposlenja.Text = uo.DatumZaposlenja.ToShortDateString();
                txtSpol.Text = uo.Spol.ToString();
                SlikaPodaciUpravnici.Source = uo.Slika.Source;
                o.Klik = false;
            }
        }

        private void PanelCuvari_MouseDown(object sender, MouseButtonEventArgs e) {
            regulisiPanel(sender as WrapPanel);
            OsobaKontrola o = dajKliknutuKontrolu(sender as WrapPanel);
            if (o == null) return;
            if (o.Klik) {
                Cuvar c = o.dajOsobu() as Cuvar;
                txtImeCuvari.Text = c.Ime;
                txtPrezimeCuvari.Text = c.Prezime;
                txtAdresaCuvari.Text = c.Adresa;
                txtIDCuvari.Text = c.Id.ToString();
                txtDatumRodjenjaCuvari.Text = c.DatumRodjenja.ToShortDateString();
                txtBlokCuvari.Text = c.Blok.ToString();
                txtNapomeneCuvari.Text = c.Napomene;
                txtDatumZaposlenjaCuvari.Text = c.DatumZaposlenja.ToShortDateString();
                txtSpolCuvari.Text = c.Spol.ToString();
                SlikaPodaciCuvari.Source = c.Slika.Source;
                txtZaduzenjaCuvari.Text = c.Zaduzenje;
                o.Klik = false;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e) {
            OsobaKontrola o = null;
            switch (TabControlBlokovi.SelectedIndex) {
                case 0: o = dajSelektovanuKontrolu(PanelBlokA); break;
                case 1: o = dajSelektovanuKontrolu(PanelBlokB); break;
                case 2: o = dajSelektovanuKontrolu(PanelBlokC); break;
                case 3: o = dajSelektovanuKontrolu(PanelSamica); break;
                case 4: o = dajSelektovanuKontrolu(PanelSmrt); break;
                case 5: o = dajSelektovanuKontrolu(PanelArhiv); break;
                case 6: o = dajSelektovanuKontrolu(PanelUslovno); break;
            }
            if (o == null) {
                System.Windows.Forms.MessageBox.Show("Morate selektovati nekog zatvorenika!");
                return;
            }
            UnosZatvorenika u = new UnosZatvorenika(o.dajOsobu() as Zatvorenik, this);
            u.Show();
        }

        private void btnOtpustanje_Click(object sender, RoutedEventArgs e) {
            OsobaKontrola o = null;
            switch (TabControlBlokovi.SelectedIndex) {
                case 0: o = dajSelektovanuKontrolu(PanelBlokA); break;
                case 1: o = dajSelektovanuKontrolu(PanelBlokB); break;
                case 2: o = dajSelektovanuKontrolu(PanelBlokC); break;
                case 3: o = dajSelektovanuKontrolu(PanelSamica); break;
                case 4: o = dajSelektovanuKontrolu(PanelSmrt); break;
            }
            if (o == null) {
                System.Windows.Forms.MessageBox.Show("Morate selektovati nekog zatvorenika!");
                return;
            }
            Otpustanje otp = new Otpustanje(o, this);
            otp.Show();
        }

        private void btnUrediUposlenika_Click(object sender, RoutedEventArgs e) {
            OsobaKontrola o = null;
            switch (tabControlUposlenici.SelectedIndex) {
                case 0: o = new OsobaKontrola(upravnik); break;
                case 1: o = dajSelektovanuKontrolu(panelUpravnici); break;
                case 2: o = dajSelektovanuKontrolu(PanelCuvari); break;
            }
            if (o == null) {
                System.Windows.Forms.MessageBox.Show("Morate selektovari nekog uposlenika!");
                return;
            }
            UnosUposlenika u = new UnosUposlenika(o, this);
            u.Show();
        }

        private void tabUslovno_GotFocus(object sender, RoutedEventArgs e) {
            btnOtpustanje.Visibility = Visibility.Hidden;
            btnOtpustanje.IsEnabled = false;
            if(admin != null) btnPovratak.Visibility = Visibility.Visible;
            if(admin != null) btnPovratak.IsEnabled = true;
        }

        private void btnPovratak_Click(object sender, RoutedEventArgs e) {
            OsobaKontrola o = null;
            switch (TabControlBlokovi.SelectedIndex) {
                case 5: o = dajSelektovanuKontrolu(PanelArhiv); break;
                case 6: o = dajSelektovanuKontrolu(PanelUslovno); break;
            }
            if (o == null) {
                System.Windows.Forms.MessageBox.Show("Morate selektovati nekog zatvorenika!");
                return;
            }
            UnosZatvorenika u = new UnosZatvorenika(o.dajOsobu() as Zatvorenik, this, true);
            u.Show();
        }

        private void tabArhiv_GotFocus(object sender, RoutedEventArgs e) {
            tabUslovno_GotFocus(sender, e);
        }

        private void tabA_GotFocus(object sender, RoutedEventArgs e) {
            if (admin != null) btnOtpustanje.Visibility = Visibility.Visible;
            if (admin != null) btnOtpustanje.IsEnabled = true;
           
        }

        private void btnDodajKor_Click(object sender, RoutedEventArgs e) {
            UnosKorisnika u = new UnosKorisnika(this);
            u.Show();
        }

        private WrapPanel dajSelektovanPanelZatvorenika() {
            switch (TabControlBlokovi.SelectedIndex) {
                case 0: return PanelBlokA;
                case 1: return PanelBlokB;
                case 2: return PanelBlokC;
                case 3: return PanelSamica;
                case 4: return PanelSmrt;
                case 5: return PanelArhiv;
                case 6: return PanelUslovno;
                default: return null;
            }
        }


        private Blok dajSelektovanBlok() {
            switch (TabControlBlokovi.SelectedIndex) {
                case 0: return Blok.BlokA;
                case 1: return Blok.BlokB;
                case 2: return Blok.BlokC;
                case 3: return Blok.Samica;
                case 4: return Blok.OdjelZaSmrtneKazne;
                default: throw new Exception("Greska!");
            }
        }
        private void pretraziBlok(Blok b, WrapPanel w, string s) {
            w.Children.Clear();
            foreach (Zatvorenik z in zatvorenici)
                if (z.Blok == b && (z.Ime.Contains(s) || z.Prezime.Contains(s))) w.Children.Add(new OsobaKontrola(z));
        }

        private void pretraziArhiv(WrapPanel w, string s) {
            w.Children.Clear();
            foreach (Arhiv a in arhivi)
                if (a.Zatvorenik.Ime.Contains(s) || a.Zatvorenik.Prezime.Contains(s)) w.Children.Add(new OsobaKontrola(a.Zatvorenik));
        }

        private void pretraziUslovne(WrapPanel w, string s) {
            w.Children.Clear();
            foreach (Arhiv a in arhiviUslovni)
                if (a.Zatvorenik.Ime.Contains(s) || a.Zatvorenik.Prezime.Contains(s)) w.Children.Add(new OsobaKontrola(a.Zatvorenik));
        }
        private void tboxPretraga_TextChanged(object sender, TextChangedEventArgs e) {
            WrapPanel p = dajSelektovanPanelZatvorenika();
            if (TabControlBlokovi.SelectedIndex == 5) pretraziArhiv(p, tboxPretraga.Text);
            else if (TabControlBlokovi.SelectedIndex == 6) pretraziUslovne(p, tboxPretraga.Text);
            else pretraziBlok(dajSelektovanBlok(), p, tboxPretraga.Text);
        }


        private void tboxPretragaCuvari_TextChanged(object sender, TextChangedEventArgs e) {
            if (tabControlUposlenici.SelectedIndex == 2) {
                if (cuvari == null) return;
                PanelCuvari.Children.Clear();
                foreach (Cuvar c in cuvari) {
                    if (c.Ime.Contains(tboxPretragaCuvari.Text) || c.Prezime.Contains(tboxPretragaCuvari.Text))
                        PanelCuvari.Children.Add(new OsobaKontrola(c));
                }
            } else tboxPretragaCuvari.Clear();
        }

        private void btnOtpustiUposlenika_Click(object sender, RoutedEventArgs e) {
            DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
            d.kreirajKonekciju();
            OsobaKontrola o = null;
            switch (tabControlUposlenici.SelectedIndex) {
                case 0: o = new OsobaKontrola(upravnik); break;
                case 1: o = dajSelektovanuKontrolu(panelUpravnici); break;
                case 2: o = dajSelektovanuKontrolu(PanelCuvari); break;
            }
            if (o == null) {
                System.Windows.Forms.MessageBox.Show("Morate selektovari nekog uposlenika!");
                return;
            }
            if (MessageBox.Show("Jeste li sigurni da želite obrisati: " + o.dajOsobu().ToString(), "Upozorenje!", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                if (tabControlUposlenici.SelectedIndex == 0) {
                    DAL_DAO.DAL.UpravnikDAO ud = d.getDAO.getUpravnikDAO();
                    ud.delete(upravnik);
                } else if (tabControlUposlenici.SelectedIndex == 1) {
                    DAL_DAO.DAL.UpravnikOdjelaDAO uod = d.getDAO.getUpravnikOdjelaDAO();
                    uod.delete(o.dajOsobu() as UpravnikOdjela);
                } else {
                    DAL_DAO.DAL.CuvarDAO cd = d.getDAO.getCuvarDAO();
                    cd.delete(o.dajOsobu() as Cuvar);
                }
                inicijalizirajUposlenike();
            }
            d.terminirajKonekciju();
        }

        private void evidentirajPrisustvo(int id) {
            DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
            d.kreirajKonekciju();
            DAL_DAO.DAL.PrisustvoDAO pd = d.getDAO.getPrisustvoDAO();
            if (pd.jeLiDosao(id)) pd.unesiOdlazak(id, DateTime.Now);
            else pd.create(new Prisustvo(id, DateTime.Now, DateTime.Now));
            d.terminirajKonekciju();
            System.Windows.Forms.MessageBox.Show("Unešeno!");
            inicijalizirajPrisustva();
        }

        private void btnOcitaj_Click(object sender, RoutedEventArgs e) {
            try {
                //WebcamCtrl.TakeSnapshot();
                string imagePath = WebcamCtrl.ImageDirectory;
                DirectoryInfo d = new DirectoryInfo(imagePath);
                FileInfo[] f = d.GetFiles();
                foreach (FileInfo fi in f) {
                    if (fi.FullName.Contains("Snapshot")) imagePath = fi.FullName;
                }
                QRCodeDecoder qrd = new QRCodeDecoder();
                QRCodeBitmapImage i = new QRCodeBitmapImage(new System.Drawing.Bitmap(imagePath));
                string s = qrd.Decode(i);
                //File.Delete(imagePath);
                evidentirajPrisustvo(Convert.ToInt32(s));                
            } catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.Message, "Problem kod čitanja koda!");
            }
        }

        private void btnDodajAdmina_Click(object sender, RoutedEventArgs e) {
            UnosAdmina u = new UnosAdmina(this);
            u.ShowDialog();
        }

        private void ubaciBlok(Blok b, List<String> s) {
            s.Add("Zatvorenici u odjelu naziva: " + b.ToString() + Environment.NewLine);
            foreach (Zatvorenik z in zatvorenici) {
                if (z.Blok == b) s.Add(z.dajPodatkeOZatvoreniku());
            }
            s.Add("********************************************************************************");
        }
        private void btnKreirajIzvj_Click(object sender, RoutedEventArgs e) {
            if (osobaZaIzvjestaj != null) {
                List<string> podaci = new List<string>();
                ubaciBlok(Blok.BlokA, podaci);
                ubaciBlok(Blok.BlokB, podaci);
                ubaciBlok(Blok.BlokC, podaci);
                ubaciBlok(Blok.Samica, podaci);
                ubaciBlok(Blok.OdjelZaSmrtneKazne, podaci);
                Izvjestaj i = new Izvjestaj(osobaZaIzvjestaj, DateTime.Now, podaci);
                i.PrintajIzvjestaj(rchIzvjestaj);
            } else System.Windows.MessageBox.Show("Morate odabrati osobu koja podnosi zahtjev za izvještaj!", "Error");
        }

        private Osoba osobaZaIzvjestaj;

        private void btnPrintaj_Click(object sender, RoutedEventArgs e) {
            PrintDialog p = new PrintDialog();
            if (p.ShowDialog() == true) {
                p.PrintDocument((((IDocumentPaginatorSource)rchIzvjestaj.Document).DocumentPaginator), "Gola Ada Izvještaj");
            }
        }

        private void btnNadjiZaIzvj_Click(object sender, RoutedEventArgs e) {
            panelKontrolaIzvjestaj.Children.Clear();
            if (numIdIzvj.Value != null) {
                if(upravnik != null)
                if (Convert.ToInt32(numIdIzvj.Value) == upravnik.Id) {
                    osobaZaIzvjestaj = upravnik;
                    panelKontrolaIzvjestaj.Children.Add(new OsobaKontrola(upravnik));
                    return;
                }
                foreach (UpravnikOdjela up in upravniciOdjela) {
                    if (up.Id == Convert.ToInt32(numIdIzvj.Value)) {
                        osobaZaIzvjestaj = up;
                        panelKontrolaIzvjestaj.Children.Add(new OsobaKontrola(up));
                        return;
                    }
                }
                foreach (Cuvar c in cuvari) {
                    if (c.Id == Convert.ToInt32(numIdIzvj.Value)) {
                        osobaZaIzvjestaj = c;
                        panelKontrolaIzvjestaj.Children.Add(new OsobaKontrola(c));
                        return;
                    }
                }
                System.Windows.Forms.MessageBox.Show("Uposlenik pod tim ID-om ne postoji!");
            } else System.Windows.MessageBox.Show("Niste unijeli ID uposlenika!");
        }


        private void rbtnMail_Checked(object sender, RoutedEventArgs e) {
            btnSaljiMailIzvj.IsEnabled = true;
            tboxMailIzvj.IsEnabled = true;
            btnPrintaj.IsEnabled = false;
        }

        private void rbtnPrint_Checked(object sender, RoutedEventArgs e) {
            try {
                btnSaljiMailIzvj.IsEnabled = false;
                tboxMailIzvj.IsEnabled = false;
                btnPrintaj.IsEnabled = true;
            } catch (Exception ex) { }
        }

        private void slanjeMaila(string kome, string tekst) {
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.AdminDAO da = d.getDAO.getAdminDAO();
                string mail = da.dajAdminovMail();
                string pass = da.dajAdminovPass();
                d.terminirajKonekciju();
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(mail, pass);
                MailMessage mm = new MailMessage(mail, kome, "Izvještaj - Gola Ada", tekst);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);
                System.Windows.MessageBox.Show("Mail je poslat!");
            } catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.Message, "Problem kod slanja maila");
            }
        }

        private void btnSaljiMailIzvj_Click(object sender, RoutedEventArgs e) {
            string tekst = new TextRange(rchIzvjestaj.Document.ContentStart, rchIzvjestaj.Document.ContentEnd).Text;
            System.Threading.Thread t = new System.Threading.Thread(salji);
            t.Start(new List<string> { tboxMailIzvj.Text, tekst });

        }

        private void salji(object obj) {
            List<string> s = obj as List<String>;
            slanjeMaila(s[0], s[1]);
        }

        private void PanelAdmini_MouseDown(object sender, MouseButtonEventArgs e) {
            regulisiPanel(sender as WrapPanel);
            OsobaKontrola o = dajKliknutuKontrolu(sender as WrapPanel);
            if (o == null) return;
            txtAdminIme.Text = (o.dajUsera() as Admin).Ime;
            txtAdminPrezime.Text = (o.dajUsera() as Admin).Prezime;
            txtAdminUser.Text = o.dajUsera().UserName;
            txtAdminMail.Text = o.dajUsera().Mail;
            o.Klik = false;
        }

        private void PanelUseri_MouseDown(object sender, MouseButtonEventArgs e) {
            regulisiPanel(sender as WrapPanel);
            OsobaKontrola o = dajKliknutuKontrolu(sender as WrapPanel);
            o.Klik = false;
        }

        private void btnUrediAdmina_Click(object sender, RoutedEventArgs e) {
            OsobaKontrola o = null;
            o = dajSelektovanuKontrolu(PanelAdmini);
            if (o != null) {
                UnosAdmina u = new UnosAdmina(o.dajUsera() as Admin, this);
                u.ShowDialog();
            } else System.Windows.Forms.MessageBox.Show("Morate selektovati nekog admina!", "Problem");
        }

        private void btnBrisiAdmina_Click(object sender, RoutedEventArgs e) {
            Admin a = null;
            a = dajSelektovanuKontrolu(PanelAdmini).dajUsera() as Admin;
            if (a != null) {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.AdminDAO ad = d.getDAO.getAdminDAO();
                ad.delete(a);
                d.terminirajKonekciju();
                inicijalizirajAdmine();
            } else System.Windows.MessageBox.Show("Morate selektovati nekog admina!", "Problem");
        }

        private void btnBrisiKor_Click(object sender, RoutedEventArgs e) {
            User u = null;
            u = dajSelektovanuKontrolu(PanelUseri).dajUsera();
            if (u != null) {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.UserDAO ud = d.getDAO.getUserDAO();
                ud.delete(u);
                d.terminirajKonekciju();
                inicijalizirajUsere();
            } else System.Windows.Forms.MessageBox.Show("Morate selektovati nekog admina!", "Problem");
        }

        private void btnPretraga_Click(object sender, RoutedEventArgs e) {
            lboxPrisustva.Items.Clear();
            foreach (Prisustvo p in prisustva) {
                if (p.Id == (int)numericPrisustvoId.Value) lboxPrisustva.Items.Add(p);
            }
        }

        private void btnPrikaziSvePrisustva_Click(object sender, RoutedEventArgs e) {
            lboxPrisustva.Items.Clear();
            foreach (Prisustvo p in prisustva) {
                 lboxPrisustva.Items.Add(p);
            }
        }

        private void btnBrisiPrisustva_Click(object sender, RoutedEventArgs e) {
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.PrisustvoDAO pd = d.getDAO.getPrisustvoDAO();
                pd.deleteAll();
                d.terminirajKonekciju();
                inicijalizirajPrisustva();
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void btnUpaliKameru_Click(object sender, RoutedEventArgs e) {
            WebcamCtrl.StartCapture();
        }

        private void btnUgasiKameru_Click(object sender, RoutedEventArgs e) {
            WebcamCtrl.StopCapture();
        }

        private void TabControlGalvni_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            /*if (TabControlGalvni.SelectedItem as TabItem == tabPrisustva) {                
                System.Windows.Forms.MessageBox.Show("Test");
                TabControlGalvni.SelectedItem = tabPrisustva;
            }*/
        }

    }
}