
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtThesis = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblType
            // 
            this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblType.Location = new System.Drawing.Point(6, 13);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(416, 21);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Definition";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblType);
            this.groupBox1.Controls.Add(this.txtThesis);
            this.groupBox1.Controls.Add(this.lblTitle);
            this.groupBox1.Location = new System.Drawing.Point(77, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 305);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // txtThesis
            // 
            this.txtThesis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtThesis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtThesis.Location = new System.Drawing.Point(15, 69);
            this.txtThesis.Multiline = true;
            this.txtThesis.Name = "txtThesis";
            this.txtThesis.ReadOnly = true;
            this.txtThesis.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtThesis.Size = new System.Drawing.Size(397, 67);
            this.txtThesis.TabIndex = 4;
            this.txtThesis.Text = "According to Wikipedia, a vaccine provides <b>immunity</b>. Do Covid Vaccines pro" +
    "vide immunity if if the vaccinated can catch Covid, get sick, pass it on and eve" +
    "n die?";
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(15, 37);
            this.lblTitle.Multiline = true;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.ReadOnly = true;
            this.lblTitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lblTitle.Size = new System.Drawing.Size(397, 26);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Wikipedia Definition of a Vaccine";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(338, 290);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "label1";
            // 
            // PuzzlePieceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "PuzzlePieceUI";
            this.Size = new System.Drawing.Size(553, 422);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtThesis;
        private System.Windows.Forms.TextBox lblTitle;
        private System.Windows.Forms.Label label1;
    }
}
