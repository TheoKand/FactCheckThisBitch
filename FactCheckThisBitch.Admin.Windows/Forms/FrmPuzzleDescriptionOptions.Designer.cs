
namespace FactCheckThisBitch.Admin.Windows.Forms
{
    partial class FrmPuzzleDescriptionOptions
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkIncludeDescriptions = new System.Windows.Forms.CheckBox();
            this.chkIncludeReferenceTitles = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstLeet = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(430, 276);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(135, 47);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(571, 276);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(135, 47);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkIncludeDescriptions
            // 
            this.chkIncludeDescriptions.AutoSize = true;
            this.chkIncludeDescriptions.Location = new System.Drawing.Point(33, 28);
            this.chkIncludeDescriptions.Name = "chkIncludeDescriptions";
            this.chkIncludeDescriptions.Size = new System.Drawing.Size(165, 24);
            this.chkIncludeDescriptions.TabIndex = 3;
            this.chkIncludeDescriptions.Text = "Include Descriptions";
            this.chkIncludeDescriptions.UseVisualStyleBackColor = true;
            // 
            // chkIncludeReferenceTitles
            // 
            this.chkIncludeReferenceTitles.AutoSize = true;
            this.chkIncludeReferenceTitles.Location = new System.Drawing.Point(33, 71);
            this.chkIncludeReferenceTitles.Name = "chkIncludeReferenceTitles";
            this.chkIncludeReferenceTitles.Size = new System.Drawing.Size(188, 24);
            this.chkIncludeReferenceTitles.TabIndex = 4;
            this.chkIncludeReferenceTitles.Text = "Include Reference Titles";
            this.chkIncludeReferenceTitles.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Leet Level:";
            // 
            // lstLeet
            // 
            this.lstLeet.FormattingEnabled = true;
            this.lstLeet.ItemHeight = 20;
            this.lstLeet.Location = new System.Drawing.Point(33, 141);
            this.lstLeet.Name = "lstLeet";
            this.lstLeet.Size = new System.Drawing.Size(199, 104);
            this.lstLeet.TabIndex = 6;
            // 
            // FrmPuzzleDescriptionOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(727, 347);
            this.Controls.Add(this.lstLeet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkIncludeReferenceTitles);
            this.Controls.Add(this.chkIncludeDescriptions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPuzzleDescriptionOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Description Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkIncludeDescriptions;
        private System.Windows.Forms.CheckBox chkIncludeReferenceTitles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstLeet;
    }
}