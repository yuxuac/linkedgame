using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LinkedGame
{
    public partial class InitializeForm : Form
    {
        public InitializeForm()
        {
            InitializeComponent();
            this.clickableLabel1.Color_On = Color.Tomato;
            this.clickableLabel1.Color_Down = Color.SteelBlue;
        }
    }
}
