using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Zatvor {
    public class Cuvar : Osoba {
        public Blok Blok { get; set; }
        public DateTime DatumZaposlenja { get; set; }
        public List<Prisustvo> Prisustva { get; set; }
        public string Zaduzenje { get; set; }
        public Cuvar() { }
        public Cuvar(string _ime, string _prezime, string _adresa, DateTime _datumRodjenja, int _id,
            Spol _spol, string _napomene, Blok _blok, DateTime _datumZaposlenja, List<Prisustvo> _prisustva, string _zaduzenje, Image slika)
            : base(_ime, _prezime, _adresa, _datumRodjenja, _id, _spol, _napomene, slika) {
            Blok = _blok;
            DatumZaposlenja = _datumZaposlenja;
            Prisustva = _prisustva;
            Zaduzenje = _zaduzenje;
        }

        public Cuvar(string ime, string prezime)
            : base(ime, prezime) {

        }
        public override string ToString() {
            return "Cuvar: " + Ime + " " + Prezime;
        }
    }
}
