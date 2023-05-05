using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Olimpiada_Csharp_2019_Nationala
{
    public partial class BibliotecarBiblioteca : Form
    {
        public BibliotecarBiblioteca(int idUtilizator)
        {
            InitializeComponent();
            butRegister.Enabled = false;
            tbName.Text = "Bibliotecar " + Program.dbInstance.Utilizatori[idUtilizator-1].NumePrenume;
            var bmp = new Bitmap("../../resurse/Imagini/utilizatori/" + idUtilizator + ".jpg");
            panel1.BackgroundImage = bmp;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            Timer tmr = new Timer();
            tmr.Interval = 1000;
            tmr.Start();
            tmr.Tick += (s, ev) => { labData.Text = DateTime.Now.ToString(); };

            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            tabControl2.SelectedIndexChanged += TabControl2_SelectedIndexChanged;


            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            dataGridView1.ReadOnly = true;
            dataGridView2.ReadOnly = true;
            dataGridView3.ReadOnly = true;
            dataGridView4.ReadOnly = true;
            dataGridView5.ReadOnly = true;


            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
            dataGridView2.CellContentClick += DataGridView2_CellContentClick;
            dataGridView3.CellContentClick += DataGridView3_CellContentClick;
            dataGridView4.CellContentClick += DataGridView4_CellContentClick;
            dataGridView5.CellContentClick += DataGridView5_CellContentClick;
            dataGridView4.CellContentDoubleClick += DataGridView4_CellContentDoubleClick;
        }


        private void DataGridView4_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var dis = (List<Carte>)dataGridView4.DataSource;
            new PrevizualizareCarte(dis[e.RowIndex]).ShowDialog();
        }

        private void DataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var dis = (List<Carte>)dataGridView4.DataSource;

            Carte ca = dis[e.RowIndex];
            if (dataGridView4.Columns[e.ColumnIndex].Name == "Rezerva")
            {
                if (currentCititor.GetRezervariActive().Count == 3)
                {
                    MessageBox.Show("Nu se poate efectua rezervarea!");
                }
                else
                {
                    Program.dbInstance.Rezervari.Add(new Rezervare
                    {
                        DataRezervare = DateTime.Now,
                        IdCarte = ca.IdCarte,
                        IdCititor = currentCititor.IdCititor,
                        IdRezervare = Program.dbInstance.Rezervari.Count + 1,
                        StatusRezervare = 1,
                    });
                }
                LoadCount();
                LoadCarti();
            }
            else if (dataGridView4.Columns[e.ColumnIndex].Name == "Imprumuta")
            {
                if (currentCititor.GetImprumuturiActive().Count == 3)
                {
                    MessageBox.Show("Nu se poate efectua imprumutul!");
                }
                else
                {
                    Program.dbInstance.Imprumuturi.Add(new Imprumut
                    {
                        DataImprumut = DateTime.Now,
                        DataRestituire = null,
                        IdCarte = ca.IdCarte,
                        IdCititor = currentCititor.IdCititor,
                        IdImprumut = Program.dbInstance.Imprumuturi.Count + 1,
                    });
                }
                LoadCarti();
                LoadCount();
            }
        }

        private void DataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var dis = (List<DisplayRezervari>)dataGridView3.DataSource; 
            if (dataGridView3.Columns[e.ColumnIndex].Name == "Anuleaza")
            {
                Program.dbInstance.Rezervari[dis[e.RowIndex].IdRezervare - 1].StatusRezervare = 0;
            }
            else if (dataGridView3.Columns[e.ColumnIndex].Name == "Imprumuta")
            {
                if(currentCititor.GetImprumuturiActive().Count == 3)
                {
                    MessageBox.Show("Nu se poate efectua imprumutul!");
                }
                else
                {
                    Program.dbInstance.Rezervari[dis[e.RowIndex].IdRezervare - 1].StatusRezervare = 0;
                    Program.dbInstance.Imprumuturi.Add(new Imprumut()
                    {
                        DataImprumut=DateTime.Now,
                        IdCarte = dis[e.RowIndex].IdCarte,
                        IdCititor = currentCititor.IdCititor,
                        IdImprumut = Program.dbInstance.Imprumuturi.Count + 1
                    });
                }
            }

            LoadCartiImprumutate();
            LoadRezervari();
            LoadCount();
        }
        private void LoadCarti(string title = "", string autor = "")
        {
            dataGridView4.Columns.Clear();
            HashSet<int> cartiRezSauImp = new HashSet<int>();
            Program.dbInstance.Rezervari.ForEach((imp)=> {
                if (imp.StatusRezervare == 1)
                    cartiRezSauImp.Add(imp.IdCarte);
            });
            Program.dbInstance.Imprumuturi.ForEach((imp)=> {
                if (imp.DataRestituire == null)
                    cartiRezSauImp.Add(imp.IdCarte);
            });
            dataGridView4.DataSource = Program.dbInstance.Carti.Where((ca)=> {
                bool ctitle = true;
                bool cautor = true;
                if(title != "")
                {
                    ctitle = ca.Titlu.ToLower().Contains(title.ToLower());
                }
                if(autor != "")
                {
                    cautor = ca.Autor.ToLower().Contains(autor.ToLower());
                }
                return cartiRezSauImp.Contains(ca.IdCarte) == false && ctitle && cautor;
                }).ToList();
            var but = new DataGridViewButtonColumn();
            but.UseColumnTextForButtonValue = true;
            but.Name = "Rezerva";
            but.Text = "Rezerva";
            dataGridView4.Columns.Add(but);
            
            but = new DataGridViewButtonColumn();
            but.UseColumnTextForButtonValue = true;
            but.Name = "Imprumuta";
            but.Text = "Imprumuta";
            dataGridView4.Columns.Add(but);
        }

        private void LoadRezervari()
        {
            dataGridView3.Columns.Clear();
            dataGridView3.DataSource = currentCititor.GetRezervariActive().Select((rez)=>new DisplayRezervari { 
                Autori = Program.dbInstance.Carti[rez.IdCarte-1].Autor,
                Titlu= Program.dbInstance.Carti[rez.IdCarte-1].Titlu,
                DataRezervare = rez.DataRezervare,
                DataExpirareRezervare = rez.DataRezervare.AddDays(1),
                IdCarte=rez.IdCarte,
                IdRezervare = rez.IdRezervare
            }).ToList();
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            var but = new DataGridViewButtonColumn();
            but.UseColumnTextForButtonValue = true;
            but.Name = "Anuleaza";
            but.Text = "Anuleaza";
            dataGridView3.Columns.Add(but);
            
            but = new DataGridViewButtonColumn();
            but.UseColumnTextForButtonValue = true;
            but.Name = "Imprumuta";
            but.Text = "Imprumuta";
            dataGridView3.Columns.Add(but);
        }

        private (int,int) LoadCount()
        {
            (int, int) remain;
            remain.Item1 = 3 - currentCititor.GetRezervariActive().Count;
            remain.Item2 = 3 - currentCititor.GetImprumuturiActive().Count;
            labRezervari.Text = "Rezervari ramase = " + remain.Item1.ToString();
            labImprumuturi.Text = "Imprumuturi ramase = " + remain.Item2.ToString();
            return remain;
        }

        private void DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Restituire")
            {
                var data = (List<DisplayImprumut>)dataGridView2.DataSource;
                var imp = data[e.RowIndex];
                Program.dbInstance.Imprumuturi[imp.IdImprumut-1].DataRestituire = DateTime.Now;
            }
            LoadCartiImprumutate();
            LoadCount();
            LoadRezervari();
        }

        private bool loadedCititor = false;

        void LoadCartiImprumutate()
        {
            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = Program.dbInstance.Imprumuturi.Where((imp) => imp.IdCititor == currentCititor.IdCititor && imp.DataRestituire == null).Select((imp) => new DisplayImprumut { 
                IdImprumut = imp.IdImprumut,
                IdCarte = imp.IdCarte,
                Titlu = Program.dbInstance.Carti[imp.IdCarte-1].Titlu,
                Autori = Program.dbInstance.Carti[imp.IdCarte - 1].Autor,
                DataImprumut = imp.DataImprumut,
                DataExpirareImprumut = imp.DataImprumut.AddDays(7)
            }).ToList();
            var butCol = new DataGridViewButtonColumn();
            butCol.Name = "Restituire";
            butCol.Text = "Restituire";
            butCol.UseColumnTextForButtonValue = true;
            dataGridView2.Columns.Add(butCol);
        }
        private void TabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCount();
            LoadRezervari();
            LoadCartiImprumutate();
            LoadCarti();
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (loadedCititor == false && tabControl1.SelectedIndex == 2)
            {
                tabControl1.SelectedIndex = 1;
                MessageBox.Show("Selecteaza un cititor main intai!");
                return;
            }
            if (tabControl1.SelectedIndex == 1)
            {
                LoadCititori();
            }
            if (tabControl1.SelectedIndex == 2)
            {
                LoadCartiImprumutate();
                LoadCount();
                LoadRezervari();
            }
        }

        private List<DisplayCititor> displayCititori;

        private void LoadCititori()
        {
            dataGridView1.Columns.Clear();
            displayCititori = Program.dbInstance.Utilizatori.Where((ut) => ut.TipUtilizator == 2).Select(ut => new DisplayCititor(ut)).ToList();
            dataGridView1.DataSource = displayCititori;
            DataGridViewButtonColumn butCol = new DataGridViewButtonColumn();
            butCol.Text = "Afiseaza";
            butCol.Name = "Afiseaza";
            butCol.UseColumnTextForButtonValue = true;
            
            dataGridView1.Columns.Add(butCol);
        }
        private DisplayCititor currentCititor;
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if(dataGridView1.Columns[e.ColumnIndex].Name == "Afiseaza")
            {
                loadedCititor = true;
                var disp = (List<DisplayCititor>)dataGridView1.DataSource;
                var cit = disp[e.RowIndex];
                labNumeCititor.Text = $"Cititor: IdCititor = {cit.IdCititor}, Nume si prenume = {cit.NumePrenume}";
                panel3.BackgroundImage = new Bitmap("../../resurse/Imagini/utilizatori/" + cit.IdCititor + ".jpg"); 
                panel3.BackgroundImageLayout = ImageLayout.Stretch;
                currentCititor = cit;
                LoadCartiImprumutate();
                tabControl1.SelectedIndex = 2;
            }
        }

        private void rbCititorChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            if (rb.Checked==false)
            {
                tbParola.Enabled = true;
                tbRepetaParola.Enabled = true;
            }
            else
            {
                tbParola.Enabled = false;
                tbRepetaParola.Enabled = false;
            }
        }

        bool CheckEmail(string email)
        {
            email = email.Trim();
            if (email.EndsWith("."))
                return false;
            if (email.Contains(".") == false)
                return false;
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        private void registerButClick(object sender, EventArgs e)
        {
            if (Program.dbInstance.Utilizatori.Any((t) => t.Email == tbEmail.Text))
                return; 
            if (butRegister.Enabled == false)
                return;
            int id = Program.dbInstance.Utilizatori.Count + 1;
            var ut = new Utilizator()
            {
                IdUtilizator = id,
                Email = tbEmail.Text,
                NumePrenume = tbNumeSiPrenume.Text,
                Parola = Program.CripteazaParola(tbParola.Text),
                TipUtilizator = radioButton1.Checked ? 1 : 2,
            };

            var pimg = panel2.BackgroundImage;
            pimg.Save("../../resurse/Imagini/utilizatori/" + id + ".jpg");
            Program.dbInstance.Utilizatori.Add(ut);
            ClearData();
        }

        private void tbTextChangedAll(object sender, EventArgs e)
        {
            butRegister.Enabled = false;
            if(radioButton1.Checked)
            if (tbParola.Text != tbRepetaParola.Text || tbParola.Text.Length < 4)
                return;

            if (CheckEmail(tbEmail.Text) == false)
                return;

            if (tbEmail.Text.Length == 0 || tbNumeSiPrenume.Text.Length == 0)
                return;

            if (panel2.BackgroundImage == null)
                return;

            butRegister.Enabled = true;

        }
        private void ClearData()
        {
            tbEmail.Text = "";
            tbRepetaParola.Text = "";
            tbNumeSiPrenume.Text = "";
            tbParola.Text = "";
            panel2.BackgroundImage = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dia= new OpenFileDialog();
            dia.DefaultExt = "*.jpg";
            dia.InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\resurse\\Imagini\\altele\\";
            dia.ShowDialog();
            if (dia.FileName.EndsWith(".jpg"))
            {
                var bmp = new Bitmap(dia.FileName);
                panel2.BackgroundImage = bmp;
                panel2.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                MessageBox.Show("Fisierul nu este jpg.");
            }
            tbTextChangedAll(null,null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(tbSearchTerm.Text == "")
            {
                dataGridView1.DataSource = displayCititori;
            }
            else
            {
                dataGridView1.DataSource = displayCititori.Where((ci) => ci.NumePrenume.ToLower().Contains(tbSearchTerm.Text.ToLower())).ToList();
            }
        }

        private void printeazaFisaCititor(object sender, EventArgs e)
        {
            PrintPreviewDialog pd = new PrintPreviewDialog();
            var doc = new System.Drawing.Printing.PrintDocument();
            doc.PrintPage += Doc_PrintPage; 
            pd.Document = doc;
            pd.ShowDialog();
        }

        private void Doc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            dataGridView2.Columns.Clear();
            var log = new Bitmap("../../resurse/Imagini/altele/biblioteca2.gif");
            e.Graphics.DrawImage(log,new Rectangle(0,0,100,100));

            dataGridView2.DataSource = Program.dbInstance.Imprumuturi.Where((imp) => imp.IdCititor == currentCititor.IdCititor).Select((imp)=>new DisplayPrint { 
                Autor= Program.dbInstance.Carti[imp.IdCarte - 1].Autor,
                DenumireCarte= Program.dbInstance.Carti[imp.IdCarte - 1].Titlu,
                DataImprumut = imp.DataImprumut,
                DataRestituirii = imp.DataRestituire,
                }).ToList();
            Size sz = new Size(dataGridView2.Width, dataGridView2.Height);
            dataGridView2.Height = e.MarginBounds.Height;
            dataGridView2.Width = e.MarginBounds.Width;
            
            Bitmap bmp = new Bitmap(e.MarginBounds.Width,e.MarginBounds.Height);
            dataGridView2.ClearSelection();
            var font = new Font("verdana", 20f);
            dataGridView2.DrawToBitmap(bmp, new Rectangle(new Point(0, 120), new Size(dataGridView2.Width, dataGridView2.Height)));
            e.Graphics.DrawString("Fişa cititorului", font, Brushes.Black, new Rectangle(120, 40, 0, 0));
            e.Graphics.DrawString($"Nume și prenume: {currentCititor.NumePrenume}", font, Brushes.Black, new Rectangle(120, 70, 0, 0));
            e.Graphics.DrawImage(bmp,e.MarginBounds);

            //reset
            dataGridView2.Height = sz.Height;
            dataGridView2.Width = sz.Width;
            LoadCartiImprumutate();
        }

        private void filtreazaClick(object sender, EventArgs e)
        {
            LoadCarti(textBox1.Text, textBox2.Text);
        }

        private int readPag;
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int amnt = 0;
            int.TryParse(textBox3.Text, out amnt);
            labRezPg.Text = " x 7 = " + (7 * amnt).ToString();
            readPag = (7 * amnt);
        }

        private void butGen_Click(object sender, EventArgs e)
        {
            LoadCarti();
            var carti = ((List<Carte>)dataGridView4.DataSource).Where((ca) => ca.Nrpag < readPag).ToList();
            List<Carte[]> c3 = new List<Carte[]>();
            List<Carte[]> c2 = new List<Carte[]>();
            List<Carte[]> c1 = new List<Carte[]>();

            for (int i = 0; i < carti.Count; i++)
            {
                int ok1 = 0;
                for(int j = i + 1; j < carti.Count; j++)
                {
                    int ok2 = 0;
                    for(int k = j + 1; k < carti.Count; k++)
                    {
                        if (carti[i].Nrpag + carti[j].Nrpag + carti[k].Nrpag <= readPag)
                        {
                            ok2 = 1;
                            c3.Add(new Carte[] { carti[i], carti[j], carti[k] });
                        }
                    }

                    if(ok2 == 0 && carti[i].Nrpag + carti[j].Nrpag <= readPag)
                    {
                        c2.Add(new Carte[] { carti[i], carti[j], null });
                        ok2 = 1;
                    }
                }

                if (ok1 == 0)
                {
                    c1.Add(new Carte[] { carti[i], null, null });
                }
            }


            List<DisplayLectura> lec3 = new List<DisplayLectura>();
            List<DisplayLectura> lec2 = new List<DisplayLectura>();
            List<DisplayLectura> lec1 = new List<DisplayLectura>();
            List<DisplayLectura> lec = new List<DisplayLectura>();
            foreach (var c in c3)
                lec3.Add(new DisplayLectura(c[0], c[1], c[2]));
            foreach (var c in c2)
                lec2.Add(new DisplayLectura(c[0], c[1], c[2]));
            foreach (var c in c1)
                lec1.Add(new DisplayLectura(c[0], c[1], c[2]));
            
            lec1 = lec1.OrderByDescending(l => l.TotalPg).ToList();
            lec2 = lec2.OrderByDescending(l => l.TotalPg).ToList();
            lec3 = lec3.OrderByDescending(l => l.TotalPg).ToList();

            lec.AddRange(lec3);
            lec.AddRange(lec2);
            lec.AddRange(lec1);
            dataGridView5.Columns.Clear();
            dataGridView5.DataSource = lec;


            var but = new DataGridViewButtonColumn();
            but.UseColumnTextForButtonValue = true;
            but.Name = "Rezerva";
            but.Text = "Rezerva";

            dataGridView5.Columns.Add(but);
            
            but = new DataGridViewButtonColumn();
            but.UseColumnTextForButtonValue = true;
            but.Name = "Imprumuta";
            but.Text = "Imprumuta";
            dataGridView5.Columns.Add(but);
        }

        private void DataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var car = (List<DisplayLectura>)(dataGridView5.DataSource);
            (var rz, var im) = LoadCount();
            var lec = car[e.RowIndex];
            if (dataGridView5.Columns[e.ColumnIndex].Name == "Imprumuta")
            {
                if(lec.cnt > im)
                {
                    MessageBox.Show("Nu poti imprumuta mai mult de 3 carti odata!");
                }
                else
                {
                    foreach(Carte ca in lec.carti)
                    {
                        if (ca != null)
                        {
                            Program.dbInstance.Imprumuturi.Add(new Imprumut
                            {
                                DataImprumut = DateTime.Now,
                                DataRestituire = null,
                                IdCarte = ca.IdCarte,
                                IdCititor = currentCititor.IdCititor,
                                IdImprumut = Program.dbInstance.Imprumuturi.Count + 1,
                            });
                        }
                    }
                    LoadCount();
                    butGen_Click(null, null);
                }
            }
            else if (dataGridView5.Columns[e.ColumnIndex].Name == "Rezerva")
            {
                if(lec.cnt > rz)
                {
                    MessageBox.Show("Nu poti rezerva mai mult de 3 carti odata!");
                }
                else
                {
                    foreach (Carte ca in lec.carti)
                    {
                        if (ca != null)
                        {
                            Program.dbInstance.Rezervari.Add(new Rezervare
                            {
                                DataRezervare = DateTime.Now,
                                IdCarte = ca.IdCarte,
                                IdCititor = currentCititor.IdCititor,
                                IdRezervare = Program.dbInstance.Rezervari.Count + 1,
                                StatusRezervare = 1,
                            });
                        }
                    }
                    LoadCount();
                    butGen_Click(null, null);
                }
            }
        }
    }
}

