
namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    partial class PuzzlePieceUi
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
            this.btnLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblType
            // 
            this.lblType.AllowDrop = true;
            this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblType.BackColor = System.Drawing.Color.Silver;
            this.lblType.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblType.Location = new System.Drawing.Point(3, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(188, 18);
            this.lblType.TabIndex = 5;
            this.lblType.Text = "Definition";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblType.DragDrop += new System.Windows.Forms.DragEventHandler(this.lblType_DragDrop);
            this.lblType.DragEnter += new System.Windows.Forms.DragEventHandler(this.lblType_DragEnter);
            this.lblType.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblType_MouseDown);
            // 
            // lblKeywords
            // 
            this.lblKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKeywords.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblKeywords.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblKeywords.ForeColor = System.Drawing.Color.Black;
            this.lblKeywords.Location = new System.Drawing.Point(0, 65);
            this.lblKeywords.Name = "lblKeywords";
            this.lblKeywords.Size = new System.Drawing.Size(194, 84);
            this.lblKeywords.TabIndex = 7;
            this.lblKeywords.Text = "one\r\ntwo\r\nthree\r\nfour\r\nfive\r\nsix";
            this.lblKeywords.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLabel
            // 
            this.btnLabel.Location = new System.Drawing.Point(3, 18);
            this.btnLabel.Name = "btnLabel";
            this.btnLabel.Size = new System.Drawing.Size(188, 47);
            this.btnLabel.TabIndex = 8;
            this.btnLabel.TabStop = true;
            this.btnLabel.Text = "one\r\ntwo\r\nthree\r\n";
            this.btnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLabel.Click += new System.EventHandler(this.btnLabel_Click_1);
            // 
            // PuzzlePieceUi
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblKeywords);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.btnLabel);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Name = "PuzzlePieceUi";
            this.Size = new System.Drawing.Size(194, 149);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblKeywords;
        private System.Windows.Forms.LinkLabel btnLabel;
    }
}
