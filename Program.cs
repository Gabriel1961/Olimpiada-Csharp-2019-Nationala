using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Olimpiada_Csharp_2019_Nationala
{
    internal static class Program
    {
        public static DbModel dbInstance;
        
        public static string CripteazaParola(string parola)
        {
            StringBuilder res = new StringBuilder(parola);
            for(int i = 0; i < parola.Length; i++)
            {
                if (parola[i] >= 'a' && parola[i] <= 'z')
                {
                    if (parola[i] == 'z')
                        res[i] = 'a';
                    else 
                        res[i] = (char)(parola[i] + 1);
                }
                else if (parola[i] >= 'A' && parola[i] <= 'Z')
                {
                    if (parola[i] == 'A')
                        res[i] = 'Z';
                    else
                        res[i] = (char)(parola[i] - 1);
                }
                else if (parola[i] >= '0' && parola[i] <= '9')
                {
                    res[i] = (char)('0' + (9 - (parola[i] - '0'))); 
                }
            }
            return res.ToString();
        }

        static void ReadDb()
        {
            XmlSerializer ser = new XmlSerializer(typeof(DbModel));
            if (File.Exists("../../Biblioteca.xml"))
            {
                var f = File.OpenRead("../../Biblioteca.xml");
                dbInstance = (DbModel)ser.Deserialize(f);
                f.Close();
                f.Dispose();
            }
            else
            {
                dbInstance = new DbModel();
                dbInstance.Imprumuturi = new List<Imprumut>();
                dbInstance.Carti = new List<Carte>();
                dbInstance.Utilizatori = new List<Utilizator>();
                dbInstance.Rezervari = new List<Rezervare>();

                var lines = File.ReadAllLines("../../resurse/utilizatori.txt");
                int i = 1;
                foreach(var line in lines)
                {
                    string[] cuv = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var ut = new Utilizator()
                    {
                        IdUtilizator = i,
                        TipUtilizator = int.Parse(cuv[0]),
                        NumePrenume = cuv[1],
                        Email = cuv[2],
                    };
                    if (ut.TipUtilizator == 1)
                    {
                        ut.Parola = CripteazaParola(cuv[3]);
                    }
                    dbInstance.Utilizatori.Add(ut);
                    i++;
                }

                lines = File.ReadAllLines("../../resurse/carti.txt");
                i = 1;
                foreach (var line in lines)
                {
                    string[] cuv = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var ca = new Carte()
                    {
                        IdCarte = i,
                        Titlu = cuv[0],
                        Autor = cuv[1],
                        Nrpag = int.Parse(cuv[2])
                    };
                    dbInstance.Carti.Add(ca);
                    i++;
                }

                lines = File.ReadAllLines("../../resurse/rezervari.txt");
                i = 1;
                foreach (var line in lines)
                {
                    string[] cuv = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var re = new Rezervare()
                    {
                        IdRezervare = i,
                        IdCititor = int.Parse(cuv[0]),
                        IdCarte = int.Parse(cuv[1]),
                        DataRezervare = DateTime.ParseExact(cuv[2], "MM/dd/yyyy hh/mm/ss tt", CultureInfo.InvariantCulture),
                        StatusRezervare = int.Parse(cuv[3]),
                    };
                    dbInstance.Rezervari.Add(re);
                    i++;
                }

                lines = File.ReadAllLines("../../resurse/imprumuturi.txt");
                i = 1;
                foreach (var line in lines)
                {
                    string[] cuv = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var im = new Imprumut()
                    {
                        IdImprumut = i,
                        IdCititor = int.Parse(cuv[0]),
                        IdCarte = int.Parse(cuv[1]),
                        DataImprumut = DateTime.ParseExact(cuv[2], "MM/dd/yyyy hh/mm/ss tt", CultureInfo.InvariantCulture),
                    };

                    if (cuv[3] == "NULL")
                        im.DataRestituire = null;
                    else
                        im.DataRestituire = DateTime.ParseExact(cuv[3], "MM/dd/yyyy hh/mm/ss tt", CultureInfo.InvariantCulture);
                    
                    dbInstance.Imprumuturi.Add(im);
                    i++;
                }
            }
        }

        static void WriteDb()
        {
            var f = File.OpenWrite("../../Biblioteca.xml");
            XmlSerializer ser = new XmlSerializer(typeof(DbModel));
            ser.Serialize(f, dbInstance);
            f.Close();
            f.Dispose();
        }

        [STAThread]
        static void Main()
        {
            ReadDb();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BibliotecarBiblioteca(1));
            WriteDb();
        }
    }
}
