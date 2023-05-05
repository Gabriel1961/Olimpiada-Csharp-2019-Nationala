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
    public partial class PrevizualizareCarte : Form
    {
        public Bitmap[] SplitImg(Bitmap bmp)
        {
            float hw = bmp.Width / 2;
            float hh = bmp.Width / 2;
            RectangleF r1 = new RectangleF(0, 0, hw,hh);
            RectangleF r2 = new RectangleF(hw, 0, hw,hh);
            RectangleF r3 = new RectangleF(0, hh, hw,hh);
            RectangleF r4 = new RectangleF(hw, hh, hw,hh);

            return new Bitmap[] {
                bmp.Clone(r1,System.Drawing.Imaging.PixelFormat.DontCare),
                bmp.Clone(r2,System.Drawing.Imaging.PixelFormat.DontCare),
                bmp.Clone(r3,System.Drawing.Imaging.PixelFormat.DontCare),
                bmp.Clone(r4,System.Drawing.Imaging.PixelFormat.DontCare),
            };
        }

        Bitmap carteImg;
        public PrevizualizareCarte(Carte carte)
        {
            InitializeComponent();
            carteImg = new Bitmap("../../resurse/Imagini/carti/" + carte.IdCarte + ".jpg");
            p0.BackgroundImage = carteImg;
            var i1 = SplitImg(carteImg);
            p1.BackgroundImage = i1[0];
            p2.BackgroundImage = i1[1];
            p3.BackgroundImage = i1[2];
            p4.BackgroundImage = i1[3];

            var a1 = SplitImg(i1[0]);
            var a2 = SplitImg(i1[1]);
            var a3 = SplitImg(i1[2]);
            var a4 = SplitImg(i1[3]);

            p11.BackgroundImage = a1[0];
            p12.BackgroundImage = a1[1];
            p13.BackgroundImage = a1[2];
            p14.BackgroundImage = a1[3];

            p21.BackgroundImage = a2[0];
            p22.BackgroundImage = a2[1];
            p23.BackgroundImage = a2[2];
            p24.BackgroundImage = a2[3];

            p31.BackgroundImage = a3[0];
            p32.BackgroundImage = a3[1];
            p33.BackgroundImage = a3[2];
            p34.BackgroundImage = a3[3];

            p41.BackgroundImage = a4[0];
            p42.BackgroundImage = a4[1];
            p43.BackgroundImage = a4[2];
            p44.BackgroundImage = a4[3];

            panel2.BackgroundImage = carteImg;

            textBox1.Text = carte.Titlu;
            textBox2.Text = carte.Autor;
            textBox3.Text = carte.Nrpag.ToString();
        }

        float zoomFactor = 1;
        private void button1_Click(object sender, EventArgs e)
        {
            zoomFactor -= .25f;
            if(zoomFactor == 0)
                zoomFactor = 1;
            int h = carteImg.Width;
            int w = carteImg.Height;
            int zh = (int)(h * zoomFactor);
            int zw = (int)(w * zoomFactor);
            panel2.BackgroundImage = carteImg.Clone(new RectangleF(w/2-zw/2,h/2-zh/2,zw,zh),System.Drawing.Imaging.PixelFormat.DontCare);
        }
    }
}
