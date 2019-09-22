using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
namespace LinkedGame
{
    public partial class Item : UserControl
    {
        private System.ComponentModel.ComponentResourceManager m_resources;
        private bool m_IsSelected = false;
        private PictureBox m_ItemPic;
        private SoundPlayer m_SP;
        public string m_Img;
        public event SelectHandler SelectedEvent;
        public bool IsSelectedNow
        { 
            get
            { 
                return m_IsSelected;
            }
        }

        public Item()
        {
            InitializeComponent();
            this.m_Img = "s_Chrysanthemum";
            m_resources = new System.ComponentModel.ComponentResourceManager(typeof(Item));
            this.Size = new System.Drawing.Size(54,54);
            this.Controls.Add(CreatePictureBox(this.m_Img));
            this.BackColor = Color.White;
            this.m_SP = new SoundPlayer(System.Environment.CurrentDirectory + @"\audio\sound_mouseclick.wav");
        }

        public Item(Size itemSize)
        {
            InitializeComponent();
            this.m_Img = "s_Chrysanthemum";
            m_resources = new System.ComponentModel.ComponentResourceManager(typeof(Item));
            this.Size = itemSize;
            this.Controls.Add(CreatePictureBox(this.m_Img));
            this.BackColor = Color.White;
            this.m_SP = new SoundPlayer(System.Environment.CurrentDirectory + @"\audio\sound_mouseclick.wav");
        }

        public void SetPicture(string imgName)
        {
            if (m_ItemPic != null)
            {
                m_ItemPic.Image = Image.FromFile(imgName);
                this.m_Img = imgName;
            }
        }

        private PictureBox CreatePictureBox(string imgName)
        {
            m_ItemPic = new PictureBox();
            m_ItemPic.Location = new System.Drawing.Point(3, 3);
            m_ItemPic.Name = "m_ItemPic";
            m_ItemPic.Size = new System.Drawing.Size(this.Size.Width - 6, this.Size.Height - 6);
            m_ItemPic.TabIndex = 0;
            if ((Image)m_resources.GetObject(imgName) != null)
            {
                m_ItemPic.Image = (Image)m_resources.GetObject(imgName);
            }
            m_ItemPic.SizeMode = PictureBoxSizeMode.StretchImage;
            m_ItemPic.TabStop = false;
            m_ItemPic.Click += new EventHandler(m_ItemPic_Click);
            return m_ItemPic;
        }

        private void m_ItemPic_Click(object sender, EventArgs args)
        {
            if (m_IsSelected)
            {
                this.BackColor = Color.White;
                m_IsSelected = false;
            }
            else 
            {
                this.BackColor = Color.Yellow;
                m_IsSelected = true;
            }

            if (m_IsSelected && SelectedEvent != null)
            {
                SelectedEvent(this, new EventArgs());
            }
        }

        public void ClearSelection()
        {
            this.BackColor = Color.White;
            m_IsSelected = false;
        }
    }
}
