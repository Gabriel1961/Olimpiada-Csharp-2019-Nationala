using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olimpiada_Csharp_2019_Nationala
{
    public class DisplayPrint
    {
        [DisplayName("Denumire carte")]
        public string DenumireCarte { get; set; }
        [DisplayName("Autor")]
        public string Autor { get; set; }
        [DisplayName("Data împrumut")]
        public DateTime DataImprumut { get; set; }
        [DisplayName("Data restituirii")]
        public DateTime? DataRestituirii{ get; set; }
    }
}
