
namespace DatabaseManager
{
    partial class DatabaseListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseListForm));
            this.lstDbName = new System.Windows.Forms.ListBox();
            this.lstDbField = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblfieldName = new System.Windows.Forms.Label();
            this.grpDescriptionKaydet = new System.Windows.Forms.GroupBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPdfKaydet = new System.Windows.Forms.Button();
            this.grpDescriptionKaydet.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstDbName
            // 
            this.lstDbName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstDbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lstDbName.FormattingEnabled = true;
            this.lstDbName.ItemHeight = 16;
            this.lstDbName.Location = new System.Drawing.Point(42, 30);
            this.lstDbName.Name = "lstDbName";
            this.lstDbName.Size = new System.Drawing.Size(285, 372);
            this.lstDbName.TabIndex = 0;
            this.lstDbName.SelectedIndexChanged += new System.EventHandler(this.lstDbName_SelectedIndexChanged);
            // 
            // lstDbField
            // 
            this.lstDbField.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstDbField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lstDbField.FormattingEnabled = true;
            this.lstDbField.ItemHeight = 16;
            this.lstDbField.Location = new System.Drawing.Point(362, 30);
            this.lstDbField.Name = "lstDbField";
            this.lstDbField.Size = new System.Drawing.Size(285, 372);
            this.lstDbField.TabIndex = 1;
            this.lstDbField.SelectedIndexChanged += new System.EventHandler(this.lstDbField_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Database Name";
            // 
            // lblfieldName
            // 
            this.lblfieldName.AutoSize = true;
            this.lblfieldName.Location = new System.Drawing.Point(359, 9);
            this.lblfieldName.Name = "lblfieldName";
            this.lblfieldName.Size = new System.Drawing.Size(103, 17);
            this.lblfieldName.TabIndex = 3;
            this.lblfieldName.Text = "Database Field";
            // 
            // grpDescriptionKaydet
            // 
            this.grpDescriptionKaydet.Controls.Add(this.btnKaydet);
            this.grpDescriptionKaydet.Controls.Add(this.txtDescription);
            this.grpDescriptionKaydet.Controls.Add(this.label2);
            this.grpDescriptionKaydet.Location = new System.Drawing.Point(805, 77);
            this.grpDescriptionKaydet.Name = "grpDescriptionKaydet";
            this.grpDescriptionKaydet.Size = new System.Drawing.Size(413, 179);
            this.grpDescriptionKaydet.TabIndex = 4;
            this.grpDescriptionKaydet.TabStop = false;
            this.grpDescriptionKaydet.Text = "Desciption Kaydetme";
            this.grpDescriptionKaydet.Visible = false;
            // 
            // btnKaydet
            // 
            this.btnKaydet.BackColor = System.Drawing.Color.YellowGreen;
            this.btnKaydet.Location = new System.Drawing.Point(154, 111);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(192, 29);
            this.btnKaydet.TabIndex = 2;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = false;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(154, 57);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(192, 22);
            this.txtDescription.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Description:";
            // 
            // btnPdfKaydet
            // 
            this.btnPdfKaydet.BackColor = System.Drawing.Color.Silver;
            this.btnPdfKaydet.Location = new System.Drawing.Point(959, 295);
            this.btnPdfKaydet.Name = "btnPdfKaydet";
            this.btnPdfKaydet.Size = new System.Drawing.Size(192, 51);
            this.btnPdfKaydet.TabIndex = 3;
            this.btnPdfKaydet.Text = "PDF Kaydet";
            this.btnPdfKaydet.UseVisualStyleBackColor = false;
            this.btnPdfKaydet.Click += new System.EventHandler(this.btnPdfKaydet_Click);
            // 
            // DatabaseListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1325, 420);
            this.Controls.Add(this.btnPdfKaydet);
            this.Controls.Add(this.grpDescriptionKaydet);
            this.Controls.Add(this.lblfieldName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstDbName);
            this.Controls.Add(this.lstDbField);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DatabaseListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DatabaseList";
            this.grpDescriptionKaydet.ResumeLayout(false);
            this.grpDescriptionKaydet.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstDbName;
        private System.Windows.Forms.ListBox lstDbField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblfieldName;
        private System.Windows.Forms.GroupBox grpDescriptionKaydet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnPdfKaydet;
    }
}