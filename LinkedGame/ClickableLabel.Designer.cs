namespace LinkedGame
{
    partial class ClickableLabel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Font = new System.Drawing.Font("Andy", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Location = new System.Drawing.Point(15, 10);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "label1";
            this.Size = new System.Drawing.Size(91, 42);
            this.TabIndex = 0;
            this.Text = "label1";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.MouseLeave += new System.EventHandler(this.label1_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
