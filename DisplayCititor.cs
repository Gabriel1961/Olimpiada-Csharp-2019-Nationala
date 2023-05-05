using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Olimpiada_Csharp_2019_Nationala
{
    public class DisplayCititor
    {
        public int IdCititor { get; set; }
        public string NumePrenume { get; set; }
        public string Email { get; set; }

        public List<Rezervare> GetRezervariActive()
        {
            return Program.dbInstance.Rezervari.Where((re) => re.IdCititor == IdCititor && re.StatusRezervare == 1 && re.DataRezervare.AddDays(1)>=DateTime.Now).ToList();
        }

        public List<Imprumut> GetImprumuturiActive()
        {
            return Program.dbInstance.Imprumuturi.Where((re) => re.IdCititor == IdCititor && re.DataRestituire == null).ToList();
        }
        public DisplayCititor(Utilizator utilizator)
        {
            Debug.Assert(utilizator.TipUtilizator == 2);
            IdCititor = utilizator.IdUtilizator;
            NumePrenume = utilizator.NumePrenume;
            Email = utilizator.Email;
        }
    }
}