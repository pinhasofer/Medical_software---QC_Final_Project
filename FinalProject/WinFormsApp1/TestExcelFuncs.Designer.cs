namespace WinFormsApp1
{
    partial class TestExcelFuncs
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
            this.btnXLRead = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnXLRead
            // 
            this.btnXLRead.Location = new System.Drawing.Point(56, 45);
            this.btnXLRead.Name = "btnXLRead";
            this.btnXLRead.Size = new System.Drawing.Size(75, 23);
            this.btnXLRead.TabIndex = 0;
            this.btnXLRead.Text = "קרא מאקסל";
            this.btnXLRead.UseVisualStyleBackColor = true;
            this.btnXLRead.Click += new System.EventHandler(this.btnXLRead_Click);
            // 
            // TestExcelFuncs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(187, 115);
            this.Controls.Add(this.btnXLRead);
            this.Name = "TestExcelFuncs";
            this.Text = "TestExcelFuncs";
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnXLRead;
    }
}