using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Zatvor {
    public enum TipAdmina { Glavni, Sporedni }
    public class Admin : User {        
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public TipAdmina Tip { get; set; }
        public Image Slika { get; set; }
        public Admin() { }
        public Admin(int id, string ime, string prezime, string user, string pass, string mail, TipAdmina tip, Image slika) : base(id, user, pass, mail) {
            Ime = ime;
            Prezime = prezime;
            Tip = tip;
            Slika = slika;
        }
    }
}
