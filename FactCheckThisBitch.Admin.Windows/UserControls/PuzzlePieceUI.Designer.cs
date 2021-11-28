
namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    partial class PuzzlePieceUI
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
            this.lblType = new System.Windows.Forms.Label();
            this.lblKeywords = new System.Windows.Forms.Label();
            this.btnLabel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblType
            // 
            this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblType.Location = new System.Drawing.Point(3, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(170, 21);
            this.lblType.TabIndex = 5;
            this.lblType.Text = "Definition";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblKeywords
            // 
            this.lblKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKeywords.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblKeywords.ForeColor = System.Drawing.Color.Blue;
            this.lblKeywords.Location = new System.Drawing.Point(0, 70);
            this.lblKeywords.Name = "lblKeywords";
            this.lblKeywords.Size = new System.Drawing.Size(176, 41);
            this.lblKeywords.TabIndex = 7;
            this.lblKeywords.Text = "Vaccine\r\nImmunity";
            this.lblKeywords.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnLabel
            // 
            this.btnLabel.Location = new System.Drawing.Point(3, 24);
            this.btnLabel.Name = "btnLabel";
            this.btnLabel.Size = new System.Drawing.Size(170, 43);
            this.btnLabel.TabIndex = 8;
            this.btnLabel.Text = "Wikipedia Definition of a Vaccine";
            this.btnLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLabel.UseVisualStyleBackColor = true;
            this.btnLabel.Click += new System.EventHandler(this.btnLabel_Click);
            // 
            // PuzzlePieceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Controls.Add(this.btnLabel);
            this.Controls.Add(this.lblKeywords);
            this.Controls.Add(this.lblType);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Name = "PuzzlePieceUI";
            this.Size = new System.Drawing.Size(176, 122);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblKeywords;
        private System.Windows.Forms.Button btnLabel;
    }
}
