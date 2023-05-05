using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olimpiada_Csharp_2019_Nationala
{
    public class DisplayRezervari
    {
        public int IdRezervare { get; set; }
        public int IdCarte { get; set; }
        public string Titlu { get; set; }
        public string Autori { get; set; }
        [DisplayName("Data Imprumut")]
        public DateTime DataRezervare { get; set; }
        [DisplayName("Data Expirare Imprumut")]
        public DateTime? DataExpirareRezervare { get; set; }
    }
}
