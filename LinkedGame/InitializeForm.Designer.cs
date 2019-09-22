namespace LinkedGame
{
    partial class InitializeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.clickableLabel1 = new LinkedGame.ClickableLabel();
            this.clickableLabel2 = new LinkedGame.ClickableLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.clickableLabel2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.clickableLabel1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(527, 432);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(65, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 90);
            this.label1.TabIndex = 0;
            this.label1.Text = "LinkedGame";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clickableLabel1
            // 
            this.clickableLabel1.AutoSize = true;
            this.clickableLabel1.BackColor = System.Drawing.Color.Transparent;
            this.clickableLabel1.Color_Down = System.Drawing.Color.Empty;
            this.clickableLabel1.Color_On = System.Drawing.Color.Empty;
            this.clickableLabel1.Color_Selected = System.Drawing.Color.Empty;
            this.clickableLabel1.Font = new System.Drawing.Font("Andy", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clickableLabel1.Location = new System.Drawing.Point(212, 185);
            this.clickableLabel1.Margin = new System.Windows.Forms.Padding(0);
            this.clickableLabel1.Name = "clickableLabel1";
            this.clickableLabel1.Size = new System.Drawing.Size(83, 42);
            this.clickableLabel1.TabIndex = 0;
            this.clickableLabel1.Text = "Easy";
            // 
            // clickableLabel2
            // 
            this.clickableLabel2.AutoSize = true;
            this.clickableLabel2.BackColor = System.Drawing.Color.Transparent;
            this.clickableLabel2.Color_Down = System.Drawing.Color.LightGreen;
            this.clickableLabel2.Color_On = System.Drawing.Color.Gold;
            this.clickableLabel2.Color_Selected = System.Drawing.Color.Green;
            this.clickableLabel2.Font = new System.Drawing.Font("Andy", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clickableLabel2.IsSelected = false;
            this.clickableLabel2.Location = new System.Drawing.Point(148, 239);
            this.clickableLabel2.Margin = new System.Windows.Forms.Padding(0);
            this.clickableLabel2.Name = "clickableLabel2";
            this.clickableLabel2.Size = new System.Drawing.Size(224, 42);
            this.clickableLabel2.TabIndex = 0;
            this.clickableLabel2.Text = "clickableLabel2";
            // 
            // InitializeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 456);
            this.Controls.Add(this.panel1);
            this.Name = "InitializeForm";
            this.Text = "InitializeForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private ClickableLabel clickableLabel1;
        private ClickableLabel clickableLabel2;
    }
}