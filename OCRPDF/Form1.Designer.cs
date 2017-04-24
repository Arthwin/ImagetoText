namespace OCRPDF
{
    partial class frmMain
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
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.txtExtracted = new System.Windows.Forms.TextBox();
            this.pbOriginal = new System.Windows.Forms.PictureBox();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.pbCrop = new System.Windows.Forms.PictureBox();
            this.btnPage = new System.Windows.Forms.Button();
            this.autoCalculate = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCrop)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(12, 4);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 9;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // txtPath
            // 
            this.txtPath.Enabled = false;
            this.txtPath.Location = new System.Drawing.Point(93, 4);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(168, 20);
            this.txtPath.TabIndex = 8;
            // 
            // txtExtracted
            // 
            this.txtExtracted.Location = new System.Drawing.Point(9, 285);
            this.txtExtracted.Multiline = true;
            this.txtExtracted.Name = "txtExtracted";
            this.txtExtracted.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtExtracted.Size = new System.Drawing.Size(250, 250);
            this.txtExtracted.TabIndex = 7;
            this.txtExtracted.WordWrap = false;
            // 
            // pbOriginal
            // 
            this.pbOriginal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbOriginal.Location = new System.Drawing.Point(9, 32);
            this.pbOriginal.Margin = new System.Windows.Forms.Padding(0);
            this.pbOriginal.Name = "pbOriginal";
            this.pbOriginal.Size = new System.Drawing.Size(250, 250);
            this.pbOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbOriginal.TabIndex = 6;
            this.pbOriginal.TabStop = false;
            this.pbOriginal.Paint += new System.Windows.Forms.PaintEventHandler(this.pbOriginal_Paint);
            this.pbOriginal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbOriginal_MouseDown);
            this.pbOriginal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbOriginal_MouseMove);
            this.pbOriginal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbOriginal_MouseUp);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(688, 4);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 26);
            this.btnCalculate.TabIndex = 5;
            this.btnCalculate.Text = "Calculate!";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // pbCrop
            // 
            this.pbCrop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCrop.Location = new System.Drawing.Point(267, 32);
            this.pbCrop.Margin = new System.Windows.Forms.Padding(0);
            this.pbCrop.Name = "pbCrop";
            this.pbCrop.Size = new System.Drawing.Size(500, 500);
            this.pbCrop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCrop.TabIndex = 10;
            this.pbCrop.TabStop = false;
            this.pbCrop.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCrop_Paint);
            this.pbCrop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbCrop_MouseDown);
            this.pbCrop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbCrop_MouseMove);
            this.pbCrop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbCrop_MouseUp);
            // 
            // btnPage
            // 
            this.btnPage.Location = new System.Drawing.Point(267, 1);
            this.btnPage.Name = "btnPage";
            this.btnPage.Size = new System.Drawing.Size(75, 26);
            this.btnPage.TabIndex = 11;
            this.btnPage.Text = "Next Page";
            this.btnPage.UseVisualStyleBackColor = true;
            this.btnPage.Click += new System.EventHandler(this.btnPage_Click);
            // 
            // autoCalculate
            // 
            this.autoCalculate.AutoSize = true;
            this.autoCalculate.Location = new System.Drawing.Point(587, 8);
            this.autoCalculate.Name = "autoCalculate";
            this.autoCalculate.Size = new System.Drawing.Size(95, 17);
            this.autoCalculate.TabIndex = 12;
            this.autoCalculate.Text = "Auto Calculate";
            this.autoCalculate.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 540);
            this.Controls.Add(this.autoCalculate);
            this.Controls.Add(this.pbCrop);
            this.Controls.Add(this.btnPage);
            this.Controls.Add(this.pbOriginal);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.txtExtracted);
            this.Controls.Add(this.btnCalculate);
            this.Name = "frmMain";
            this.Text = "PDF OCR";
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCrop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.TextBox txtExtracted;
        protected System.Windows.Forms.PictureBox pbOriginal;
        private System.Windows.Forms.Button btnCalculate;
        protected System.Windows.Forms.PictureBox pbCrop;
        private System.Windows.Forms.Button btnPage;
        private System.Windows.Forms.CheckBox autoCalculate;
    }
}

