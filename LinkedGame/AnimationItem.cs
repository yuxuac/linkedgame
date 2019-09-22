using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;
namespace LinkedGame
{
    public partial class AnimationItem : UserControl
    {
        private System.ComponentModel.ComponentResourceManager m_resources;
        private bool m_IsSelected = false;
        private PictureBox m_ItemPic;
        private SoundPlayer m_SP;
        public string m_Img;
        public event SelectHandler SelectedEvent;
        private Size m_OrginalSize;
        private Point m_OrginalLocation;
        private Size m_NewSize;
        private Point m_NewLocation;
        public bool IsSelectedNow
        { 
            get
            { 
                return m_IsSelected;
            }
        }

        public AnimationItem()
        {
            InitializeComponent();
            this.m_Img = "s_Chrysanthemum";
            m_resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationItem));
            this.Size = new System.Drawing.Size(54,54);
            this.Controls.Add(CreatePictureBox(this.m_Img));
            this.BackColor = Color.White;
            m_SP = new SoundPlayer(System.Environment.CurrentDirectory + @"\audio\sound_mouseclick.wav");
        }

        public AnimationItem(Size itemSize)
        {
            InitializeComponent();
            this.m_Img = "s_Chrysanthemum";
            m_resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationItem));
            this.Size = itemSize;
            this.Controls.Add(CreatePictureBox(this.m_Img));
            this.BackColor = Color.White;
            m_SP = new SoundPlayer(System.Environment.CurrentDirectory + @"\audio\sound_mouseclick.wav");
        }

        public void SetPicture(string imgName)
        {
            if (m_ItemPic != null)
            {
                if (imgName.Length > 0)
                {
                    m_ItemPic.Image = Image.FromFile(imgName);
                }
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
            m_ItemPic.MouseMove += new MouseEventHandler(AnimationItem_MouseMove);
            m_ItemPic.MouseLeave += new EventHandler(AnimationItem_MouseLeave);
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
                this.m_SP.Play();
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

        private void AnimationItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_NewLocation.Equals(new Point(0, 0)))
            {
                m_NewLocation = new Point(this.Location.X - 3, this.Location.Y - 3);
            }
            if (m_NewSize.Equals(new Size(0, 0)))
            {
                int width = Convert.ToInt32(this.Size.Width * 1.1);
                int height = Convert.ToInt32(this.Size.Height * 1.1);
                m_NewSize = new Size(width,height);
            }
            this.Size = m_NewSize;
            this.Location = m_NewLocation;
            m_ItemPic.Size = new System.Drawing.Size(this.Size.Width - 6, this.Size.Height - 6);
        }

        private void AnimationItem_MouseLeave(object sender, EventArgs e)
        {
            this.Size = m_OrginalSize;
            this.Location = m_OrginalLocation;
            m_ItemPic.Size = new System.Drawing.Size(this.Size.Width - 6, this.Size.Height - 6);
        }

        private void AnimationItem_Load(object sender, EventArgs e)
        {
            m_OrginalLocation = this.Location;
            m_OrginalSize = this.Size;
        }

        public void RefreshOrignalLocation()
        {
            m_OrginalLocation = this.Location;
            m_OrginalSize = this.Size;
            m_NewSize = new Size(0, 0);
            m_NewLocation = new Point(0,0);
        }
    }
}
