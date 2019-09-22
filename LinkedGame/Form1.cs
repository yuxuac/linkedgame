using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
namespace LinkedGame
{
    public partial class Form1 : Form
    {
        Point pStart;
        Point pMid1;
        Point pMid2;
        Point pEnd;
        bool IsSecondClick = false;
        Graphics gp;
        Pen blackPen;
        public Form1()
        {
            InitializeComponent();
            pStart = new System.Drawing.Point(0, 0);
            pMid1 = new System.Drawing.Point(0, 0);
            pMid2 = new System.Drawing.Point(0, 0);
            pEnd = new System.Drawing.Point(0, 0);
            gp = this.panel1.CreateGraphics();
            blackPen = new Pen(Color.Black, 2f);

            this.timer1.Interval = 10;
            this.timer1.Start();
            this.timer2.Interval = 1000;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label1.Text = this.panel1.PointToClient(Cursor.Position).X.ToString()+" /" +this.panel1.PointToClient(Cursor.Position).Y.ToString();
            this.label2.Text = "pStart:X=" + pStart.X + "/" + "Y=" + pStart.Y;
            this.label3.Text = "pEnd:X=" + pEnd.X + "/" + "Y=" + pEnd.Y;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (IsSecondClick)
            {
                pEnd = this.panel1.PointToClient(Cursor.Position);
                drawLine();
                IsSecondClick = false;
            }
            else
            {
                pStart = this.panel1.PointToClient(Cursor.Position);
                IsSecondClick = true;
            }
        }

        private void drawLine()
        {
            if (gp != null 
                && IsSecondClick
                && pStart != new System.Drawing.Point(0, 0))
            {
                gp.DrawLine(blackPen, pStart, pEnd);
                this.timer2.Interval = 1000;
                this.timer2.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (gp != null)
            {
                gp.Clear(Color.White);
                this.timer2.Stop();
            }
        }

        private void clickableLabel1_Click(object sender, EventArgs e)
        {
            this.panel1.BackColor = Color.Red;
        }
    }
}
