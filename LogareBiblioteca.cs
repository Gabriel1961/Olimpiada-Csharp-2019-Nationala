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
            if (Program.dbInstance.Utilizatori.Any((u)=>u.Parola == pass && u.Email == em))
            {

            }
            else
            {
                MessageBox.Show("“Email si/ sau parola invalida!");
            }
        }
    }
}
