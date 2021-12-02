
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PuzzlePieceUi));
            this.lblType = new System.Windows.Forms.Label();
            this.lblKeywords = new System.Windows.Forms.Label();
            this.btnLabel = new System.Windows.Forms.LinkLabel();
            this.dotTop = new System.Windows.Forms.PictureBox();
            this.dotRight = new System.Windows.Forms.PictureBox();
            this.dotBottom = new System.Windows.Forms.PictureBox();
            this.dotLeft = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dotTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dotRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dotBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dotLeft)).BeginInit();
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
            this.lblType.Size = new System.Drawing.Size(92, 24);
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
            this.lblKeywords.Location = new System.Drawing.Point(0, 87);
            this.lblKeywords.Name = "lblKeywords";
            this.lblKeywords.Size = new System.Drawing.Size(222, 112);
            this.lblKeywords.TabIndex = 7;
            this.lblKeywords.Text = "one\r\ntwo\r\nthree\r\nfour\r\nfive\r\nsix";
            this.lblKeywords.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnLabel
            // 
            this.btnLabel.Location = new System.Drawing.Point(3, 24);
            this.btnLabel.Name = "btnLabel";
            this.btnLabel.Size = new System.Drawing.Size(215, 63);
            this.btnLabel.TabIndex = 8;
            this.btnLabel.TabStop = true;
            this.btnLabel.Text = "one\r\ntwo\r\nthree\r\n";
            this.btnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLabel.Click += new System.EventHandler(this.btnLabel_Click_1);
            // 
            // dotTop
            // 
            this.dotTop.Image = ((System.Drawing.Image)(resources.GetObject("dotTop.Image")));
            this.dotTop.Location = new System.Drawing.Point(101, -2);
            this.dotTop.Name = "dotTop";
            this.dotTop.Size = new System.Drawing.Size(20, 23);
            this.dotTop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.dotTop.TabIndex = 9;
            this.dotTop.TabStop = false;
            this.dotTop.Visible = false;
            // 
            // dotRight
            // 
            this.dotRight.Image = ((System.Drawing.Image)(resources.GetObject("dotRight.Image")));
            this.dotRight.Location = new System.Drawing.Point(202, 87);
            this.dotRight.Name = "dotRight";
            this.dotRight.Size = new System.Drawing.Size(20, 23);
            this.dotRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.dotRight.TabIndex = 10;
            this.dotRight.TabStop = false;
            this.dotRight.Visible = false;
            // 
            // dotBottom
            // 
            this.dotBottom.Image = ((System.Drawing.Image)(resources.GetObject("dotBottom.Image")));
            this.dotBottom.Location = new System.Drawing.Point(101, 176);
            this.dotBottom.Name = "dotBottom";
            this.dotBottom.Size = new System.Drawing.Size(20, 23);
            this.dotBottom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.dotBottom.TabIndex = 11;
            this.dotBottom.TabStop = false;
            this.dotBottom.Visible = false;
            // 
            // dotLeft
            // 
            this.dotLeft.Image = ((System.Drawing.Image)(resources.GetObject("dotLeft.Image")));
            this.dotLeft.Location = new System.Drawing.Point(0, 87);
            this.dotLeft.Name = "dotLeft";
            this.dotLeft.Size = new System.Drawing.Size(20, 23);
            this.dotLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.dotLeft.TabIndex = 12;
            this.dotLeft.TabStop = false;
            this.dotLeft.Visible = false;
            // 
            // PuzzlePieceUi
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.dotLeft);
            this.Controls.Add(this.dotBottom);
            this.Controls.Add(this.dotRight);
            this.Controls.Add(this.dotTop);
            this.Controls.Add(this.lblKeywords);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.btnLabel);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PuzzlePieceUi";
            this.Size = new System.Drawing.Size(222, 199);
            ((System.ComponentModel.ISupportInitialize)(this.dotTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dotRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dotBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dotLeft)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblKeywords;
        private System.Windows.Forms.LinkLabel btnLabel;
        private System.Windows.Forms.PictureBox dotTop;
        private System.Windows.Forms.PictureBox dotRight;
        private System.Windows.Forms.PictureBox dotBottom;
        private System.Windows.Forms.PictureBox dotLeft;
    }
}
