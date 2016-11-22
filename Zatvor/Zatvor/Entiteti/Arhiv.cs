using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zatvor {
    public class Arhiv {
        public Zatvorenik Zatvorenik { get; set; }
        public string RazlogArhiviranja { get; set; }
        public DateTime DatumArhiviranja { get; set; }
        public Arhiv() { }
        public Arhiv(Zatvorenik zatvorenik, DateTime datumArhiviranja, string razlogArhiviranja) {
            Zatvorenik = zatvorenik;
            RazlogArhiviranja = razlogArhiviranja;
            DatumArhiviranja = datumArhiviranja;
        }

        public override string ToString() {
            return Zatvorenik.ToString() + " " + DatumArhiviranja.ToShortDateString();
        }
    }
  }

