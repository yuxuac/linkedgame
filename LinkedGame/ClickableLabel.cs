using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace LinkedGame
{
    public partial class ClickableLabel :Label
    {
        private Color previousColor;
        private Color onColor;
        private Color downColor;
        private Color selectedcolor;
        private bool isSelected;

        public bool IsSelected 
        {
            get { return isSelected; }
            set 
            { 
                isSelected = value;
                if (isSelected == true)
                {
                    this.BackColor = selectedcolor;
                }
                else
                {
                    this.BackColor = previousColor;
                }
            }
        }

        public Color PreviousColor
        {
            get { return previousColor; }
            set { previousColor = value; }
        }

        public Color Color_On
        {
            get { return onColor; }
            set { onColor = value; }
        }

        public Color Color_Selected
        {
            get { return selectedcolor; }
            set { selectedcolor = value; }
        }

        public Color Color_Down
        {
            get { return downColor; }
            set { downColor = value; }
        }

        public ClickableLabel()
        {
            InitializeComponent();
            this.previousColor = this.BackColor;
            this.Color_On = Color.Gold;
            this.Color_Selected = Color.Green;
            this.Color_Down = Color.LightGreen;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            if (previousColor != null)
            {
                if (isSelected == true)
                {
                    this.BackColor = Color_Selected;
                }
                else
                {
                    this.BackColor = previousColor;
                }
            }
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Color_On != null)
            {
                this.BackColor = Color_On;
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsSelected)
            {
                this.isSelected = false;
                this.BackColor = Color_Selected;
            }
            else
            {
                IsSelected = true;
                this.BackColor = previousColor;
            }
        }
    }
}
