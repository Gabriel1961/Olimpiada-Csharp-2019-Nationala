using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Olimpiada_Csharp_2019_Nationala
{
    public class DisplayLectura
    {
        public int cnt = 0;
        public Carte[] carti;
        public DisplayLectura(Carte c1, Carte c2,Carte c3)
        {
            carti = new Carte[] { c1, c2, c3 };
            if (c1 != null)
            {
                cnt++;
                IdCarte1 = c1.IdCarte.ToString();
                NrPag1 = c1.Nrpag.ToString();
                Titlu1 = c1.Titlu;
                Autor1 = c1.Autor;
            }
            if (c2 != null)
            {
                cnt++;
                IdCarte2 = c2.IdCarte.ToString();
                NrPag2 = c2.Nrpag.ToString();
                Titlu2 = c2.Titlu;
                Autor2 = c2.Autor;
            }
            if (c3!=null)
            {
                cnt++;
                IdCarte3 = c3.IdCarte.ToString();
                NrPag3 = c3.Nrpag.ToString();
                Titlu3 = c3.Titlu;
                Autor3 = c3.Autor;
            }
        }

        [DisplayName("Id Carte 1")]
        public string IdCarte1 { get; set; } = "-";
        [DisplayName("Titlu 1")]
        public string Titlu1{ get; set; } = "-";
        [DisplayName("Autor 1")]
        public string Autor1{ get; set; } = "-";
        [DisplayName("NrPag 1")]
        public string NrPag1 { get; set; } = "-";


        [DisplayName("Id Carte 2")]
        public string IdCarte2 { get; set; } = "-";
        [DisplayName("Titlu 2")]
        public string Titlu2 { get; set; } = "-";
        [DisplayName("Autor 2")]
        public string Autor2 { get; set; } = "-";
        [DisplayName("NrPag 2")]
        public string NrPag2 { get; set; } = "-";


        [DisplayName("Id Carte 3")]
        public string IdCarte3 { get; set; } = "-";
        [DisplayName("Titlu 3")]
        public string Titlu3 { get; set; } = "-";
        [DisplayName("Autor 3")]
        public string Autor3 { get; set; } = "-";
        [DisplayName("NrPag 3")]
        public string NrPag3 { get; set; } = "-";

        [DisplayName("Total pagini")]
        public int TotalPg { 
            get {
                int r1, r2, r3;
                int.TryParse(NrPag1, out r1);
                int.TryParse(NrPag2, out r2);
                int.TryParse(NrPag3, out r3);
                return r1+r2+r3; 
            }
        }
    }
}
