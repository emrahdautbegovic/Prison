using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Zatvor.DAL_DAO {    
    public partial class DAL {        
        private static MySqlConnection con = null;

        private static DAL instanca = null;
        public static DAL Instanca {
            get { return (instanca == null) ? instanca = new DAL() : instanca; }
        }
        private DAL() { }
        ~DAL() { terminirajKonekciju(); }

        public DAOFactory getDAO // mozemo napraviti i getDAO(tipBaze) koja vraca npr DAOMySqlFactory i sl. zavisi od potrebe
        {
            get { return DAOFactory.Instanca; }
        }

        public void kreirajKonekciju(string host = "localhost", string db = "mydb", string user = "root", string pass = "") {
            if (con != null) return;

            string connectionString = "server=" + host + ";user=" + user + ";pwd=" + pass + ";database=" + db + "; Convert Zero Datetime=True";
            con = new MySqlConnection(connectionString);

            try {
                if(con != null) con.Open();
            } catch (Exception e) {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }; 
        }

        public void terminirajKonekciju() {
            try {
                if (con != null) { con.Close(); con = null; }
            } catch (Exception e) { throw e; }
        }
        public static Image dajSlikuOdBajta(int kolona, MySqlDataReader r) {
            long size = r.GetBytes(kolona, 0, null, 0, 0);
            byte[] bajti = new byte[size];
            r.GetBytes(kolona, 0, bajti, 0, (int)size);
            if (bajti == null || bajti.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(bajti)) {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            Image slika = new Image();
            slika.Source = image;
            return slika;
        }
        public static byte[] dajByteOdSlike(Image slika) {
            BitmapImage bi = slika.Source as BitmapImage;
            byte[] bajti;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bi));
            using (MemoryStream ms = new MemoryStream()) {
                encoder.Save(ms);
                bajti = ms.ToArray();
            }
            return bajti;   
        }
    }
}
