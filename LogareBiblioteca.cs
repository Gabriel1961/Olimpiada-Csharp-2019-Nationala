using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Olimpiada_Csharp_2019_Nationala
{
    public partial class LogareBiblioteca : Form
    {
        public LogareBiblioteca()
        {
            InitializeComponent();
        }

        private void renuntaClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void logareClick(object sender, EventArgs e)
        {
            string pass = Program.CripteazaParola(textBox2.Text);
            string em = textBox1.Text;
            var ut = Program.dbInstance.Utilizatori.Find((u) => u.Parola == pass && u.Email == em);
            if (ut != null)
            {
                var form = new BibliotecarBiblioteca(ut.IdUtilizator);
                form.Show();
                this.Hide();
                form.FormClosed += (s, ev) => { this.Close(); };
            }
            else
            {
                MessageBox.Show("“Email si/ sau parola invalida!");
            }
        }
    }
}
