using System;
using System.Collections.Generic;

namespace Olimpiada_Csharp_2019_Nationala
{
    public class DbModel
    {
        public List<Utilizator> Utilizatori { get; set; }
        public List<Carte> Carti { get; set; }
        public List<Rezervare> Rezervari { get; set; }
        public List<Imprumut> Imprumuturi { get; set; }
    }

    public class Utilizator
    {
        public int IdUtilizator { get; set; }
        public int TipUtilizator { get; set; }
        public string NumePrenume { get; set; }
        public string Email { get; set; }   
        public string Parola { get; set; }
    }

    public class Carte
    {
        public int IdCarte { get; set; }
        public string Titlu { get; set; }
        public string Autor { get; set; }
        public int Nrpag { get; set; }  
    }

    public class Rezervare
    {
        public int IdRezervare { get; set; }
        public int IdCititor { get; set; }
        public int IdCarte { get; set; }
        public DateTime DataRezervare { get; set; }
        public int StatusRezervare { get; set; }
    }

    public class Imprumut
    {
        public int IdImprumut { get; set; }
        public int IdCititor { get; set; }
        public int IdCarte { get; set; }
        public DateTime DataImprumut { get; set; }
        public DateTime? DataRestituire { get; set; }
    }
}