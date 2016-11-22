using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Zatvor {
    public enum Spol { Musko, Zensko }
    public enum Blok { BlokA, BlokB, BlokC, Samica, OdjelZaSmrtneKazne }
    public abstract class Osoba {
        public Osoba(string _ime, string _prezime, string _adresa, DateTime _datumRodjenja, int _id, Spol _spol, string _napomene, Image slika) {
            Ime = _ime;
            Prezime = _prezime;
            Adresa = _adresa;
            DatumRodjenja = _datumRodjenja;
            Id = _id;
            Spol = _spol;
            Napomene = _napomene;
            Slika = slika;
        }
        public Osoba(string ime, string prezime) {
            Ime = ime;
            Prezime = prezime;
        }
        public Osoba() { }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Adresa { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public int Id { get; set; }
        public Spol Spol { get; set; }
        public string Napomene { get; set; }
        public Image Slika { get; set; }

        public override string ToString() {
            return Ime + " " + Prezime;

        }
        
        public static int dajIdBloka(Blok b) {
            return Convert.ToInt32(b);
        }

    }
}
