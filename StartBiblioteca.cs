using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Olimpiada_Csharp_2019_Nationala
{
    public partial class StartBiblioteca : Form
    {
        public StartBiblioteca()
        {
            InitializeComponent();
            label2.BackColor = Color.FromArgb(200, Color.White);
            button1.BackColor = Color.FromArgb(200, Color.White);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f = new LogareBiblioteca();
            f.FormClosed += (s,ev) => this.Show();
            f.Show();
            this.Hide();
        }
    }
}
