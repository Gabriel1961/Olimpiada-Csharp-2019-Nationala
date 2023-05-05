using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olimpiada_Csharp_2019_Nationala
{
    public class DisplayImprumut
    {
        public int IdImprumut { get; set; }
        public int IdCarte { get; set; }
        public string Titlu { get; set; }
        public string Autori { get; set; }
        [DisplayName("Data Imprumut")]
        public DateTime DataImprumut { get; set; }
        [DisplayName("Data Expirare Imprumut")]
        public DateTime? DataExpirareImprumut { get; set; }
    }
}
