using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace visual_hyperinterval
{
    public partial class Form1 : Form
    {
        private double MAX_EXPONENT_THREE = 3486784401.0;
        private List<uint> Points = new List<uint>();
        private List<uint> PointsHyp = new List<uint>();
        public Form1()
        {
            InitializeComponent();
        }

        private void build_Click(object sender, EventArgs e)
        {
            string path1 = @"D:\materials\projects\visual_hyperinterval\points.txt";
            string path2 = @"D:\materials\projects\visual_hyperinterval\hyp.txt";

            using (StreamReader sr1 = new StreamReader(path1, System.Text.Encoding.Default))
            {
                string line1;
                while ((line1 = sr1.ReadLine()) != null)
                {
                    Points.Add(uint.Parse(line1));
                }
            }

            using (StreamReader sr2 = new StreamReader(path2, System.Text.Encoding.Default))
            {
                string line2;
                while ((line2 = sr2.ReadLine()) != null)
                {
                    PointsHyp.Add(uint.Parse(line2));
                }
            }

            pictureBox1.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            System.Drawing.Rectangle rect;
            rect = new Rectangle(0, 0, pictureBox1.Width - 1, pictureBox1.Height - 1);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            SolidBrush redBrush = new SolidBrush(Color.Red);
            Pen blackPen = new Pen(Color.Black);
            Pen blackPen2 = new Pen(Color.Black, 1);
            Pen bluePen = new Pen(Color.Blue);
            Pen bluePen2 = new Pen(Color.DarkBlue, 2);
            gr.DrawRectangle(blackPen, rect);

            int u1 = 0;
            int u2 = 0;
            int v1 = 0;
            int v2 = 0;

            for (int i = 0; i < PointsHyp.Count - 3; i += 4)
            {
                // считываем точку из списка и приводим её к координатам pictureBox1
                u1 = (int)((double)PointsHyp[i] / MAX_EXPONENT_THREE * (pictureBox1.Width - 1));
                u2 = (int)((double)PointsHyp[i + 1] / MAX_EXPONENT_THREE * (pictureBox1.Height - 1));
                v1 = (int)((double)PointsHyp[i + 2] / MAX_EXPONENT_THREE * (pictureBox1.Width - 1));
                v2 = (int)((double)PointsHyp[i + 3] / MAX_EXPONENT_THREE * (pictureBox1.Height - 1));

                // выполняем преобразование поворота и сдвиг
                u2 = (pictureBox1.Height - 1) - u2;
                v2 = (pictureBox1.Height - 1) - v2;

                if ((u1 < v1) && (u2 < v2))
                {
                    rect = new Rectangle(u1, u2, v1 - u1, v2 - u2);
                    gr.DrawRectangle(blackPen, rect);
                }
                else if ((v1 < u1) && (v2 < u2))
                {
                    rect = new Rectangle(v1, v2, u1 - v1, u2 - v2);
                    gr.DrawRectangle(blackPen, rect);
                } 
                else if ((u1 < v1) && (u2 > v2))
                {
                    int tmp = u1;
                    u1 = v1;
                    v1 = tmp;

                    rect = new Rectangle(v1, v2, u1 - v1, u2 - v2);
                    gr.DrawRectangle(blackPen, rect);
                }
                else if ((u1 > v1) && (u2 < v2))
                {
                    int tmp = u1;
                    u1 = v1;
                    v1 = tmp;

                    rect = new Rectangle(u1, u2, v1 - u1, v2 - u2);
                    gr.DrawRectangle(blackPen, rect);
                }
            }

            for (int i = 0; i < Points.Count - 1; i += 2)
            {
                // считываем точку из списка и приводим её к координатам pictureBox1
                u1 = (int)((double)Points[i] / MAX_EXPONENT_THREE * (pictureBox1.Width - 1));
                u2 = (int)((double)Points[i + 1] / MAX_EXPONENT_THREE * (pictureBox1.Height - 1));

                // выполняем преобразование поворота и сдвиг
                u2 = (pictureBox1.Height - 1) - u2;

                //gr.FillEllipse(redBrush, u1 - 3.0f, u2 - 3.0f, 3.0f * 2, 3.0f * 2);
                //gr.DrawEllipse(bluePen2, u1 - 3.0f, u2 - 3.0f, 3.0f * 2, 3.0f * 2);

                gr.FillEllipse(redBrush, u1 - 2.5f, u2 - 2.5f, 2.5f * 2, 2.5f * 2);
                gr.DrawEllipse(blackPen2, u1 - 2.5f, u2 - 2.5f, 2.5f * 2, 2.5f * 2);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
