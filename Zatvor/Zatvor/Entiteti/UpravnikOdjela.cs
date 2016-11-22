using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Zatvor {
    public class UpravnikOdjela : Upravnik {       
        public Blok Blok { get; set; }
        public UpravnikOdjela(string ime, string prezime, string adresa, DateTime datumRodjenja, int id, Spol spol, string napomene, DateTime datumZaposlenja, List<Prisustvo> prisustva, Blok blok, Image slika) :
            base(ime, prezime, adresa, datumRodjenja, id, spol, napomene, datumZaposlenja, prisustva, slika) {
            Blok = blok;
        }

        public UpravnikOdjela(string ime, string prezime): base(ime, prezime) {
          
        }
    }
}
