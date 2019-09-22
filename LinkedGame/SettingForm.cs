using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace LinkedGame
{
    public partial class SettingForm : Form
    {
        private string[] outputFileList;
        public SettingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryInfo di = new DirectoryInfo(this.folderBrowserDialog1.SelectedPath);
                FileInfo[] fiList = di.GetFiles("*.jpg");

                string dirPath = System.Environment.CurrentDirectory + @"\images";

                if (!new DirectoryInfo(dirPath).Exists)
                {
                    Directory.CreateDirectory(dirPath);
                }

                outputFileList = new string[fiList.Count<FileInfo>()];
                for (int index = 0; index < outputFileList.Count<string>(); index++)
                {
                    outputFileList[index] = dirPath + @"\p" + (index + 1).ToString() + ".jpg";
                }

                for (int index = 0; index < outputFileList.Count<string>(); index++)
                {
                    Image imageNeeded = Helpers.ResizeImageWithoutRatio(Image.FromFile(fiList[index].FullName), new Size(64, 64));
                    imageNeeded.Save(outputFileList[index], System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                MessageBox.Show("Finished!");

                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                prc.StartInfo.FileName = dirPath;
                prc.Start();
            }
        }

        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
