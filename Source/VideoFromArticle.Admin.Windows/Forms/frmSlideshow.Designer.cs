
namespace VideoFromArticle.Admin.Windows.Forms
{
    partial class FrmSlideshow
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddArticle = new System.Windows.Forms.LinkLabel();
            this.lstArticles = new System.Windows.Forms.ListBox();
            this.btnDelete = new System.Windows.Forms.LinkLabel();
            this.btnRender = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panelSlideshow = new System.Windows.Forms.Panel();
            this.btnOutputFolder = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.LinkLabel();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnSortByDate = new System.Windows.Forms.Button();
            this.btnGenerateNarrations = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTotals = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panelSlideshow.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Info;
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1150, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitle.Location = new System.Drawing.Point(79, 71);
            this.txtTitle.Margin = new System.Windows.Forms.Padding(4);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.ReadOnly = true;
            this.txtTitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTitle.Size = new System.Drawing.Size(1043, 26);
            this.txtTitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 71);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Title";
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.SystemColors.Menu;
            this.txtId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtId.Location = new System.Drawing.Point(79, 34);
            this.txtId.Margin = new System.Windows.Forms.Padding(4);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(433, 26);
            this.txtId.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 155);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Articles";
            // 
            // btnAddArticle
            // 
            this.btnAddArticle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddArticle.AutoSize = true;
            this.btnAddArticle.Location = new System.Drawing.Point(977, 155);
            this.btnAddArticle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnAddArticle.Name = "btnAddArticle";
            this.btnAddArticle.Size = new System.Drawing.Size(38, 20);
            this.btnAddArticle.TabIndex = 2;
            this.btnAddArticle.TabStop = true;
            this.btnAddArticle.Text = "Add";
            this.btnAddArticle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnAddArticle_LinkClicked);
            // 
            // lstArticles
            // 
            this.lstArticles.AllowDrop = true;
            this.lstArticles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstArticles.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lstArticles.FormattingEnabled = true;
            this.lstArticles.HorizontalScrollbar = true;
            this.lstArticles.ItemHeight = 19;
            this.lstArticles.Location = new System.Drawing.Point(7, 179);
            this.lstArticles.Margin = new System.Windows.Forms.Padding(4);
            this.lstArticles.Name = "lstArticles";
            this.lstArticles.Size = new System.Drawing.Size(1114, 346);
            this.lstArticles.TabIndex = 4;
            this.lstArticles.SelectedIndexChanged += new System.EventHandler(this.lstArticles_SelectedIndexChanged);
            this.lstArticles.DoubleClick += new System.EventHandler(this.lstArticles_DoubleClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.AutoSize = true;
            this.btnDelete.Location = new System.Drawing.Point(1044, 155);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(56, 20);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.TabStop = true;
            this.btnDelete.Text = "Delete";
            this.btnDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnDelete_LinkClicked);
            // 
            // btnRender
            // 
            this.btnRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRender.Location = new System.Drawing.Point(759, 535);
            this.btnRender.Name = "btnRender";
            this.btnRender.Size = new System.Drawing.Size(118, 48);
            this.btnRender.TabIndex = 6;
            this.btnRender.Text = "Render";
            this.btnRender.UseVisualStyleBackColor = true;
            this.btnRender.Click += new System.EventHandler(this.btnRender_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 36);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Id";
            // 
            // panelSlideshow
            // 
            this.panelSlideshow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSlideshow.Controls.Add(this.btnOutputFolder);
            this.panelSlideshow.Controls.Add(this.btnRefresh);
            this.panelSlideshow.Controls.Add(this.btnPlay);
            this.panelSlideshow.Controls.Add(this.btnOpenFolder);
            this.panelSlideshow.Controls.Add(this.btnSortByDate);
            this.panelSlideshow.Controls.Add(this.btnGenerateNarrations);
            this.panelSlideshow.Controls.Add(this.label4);
            this.panelSlideshow.Controls.Add(this.lblTotals);
            this.panelSlideshow.Controls.Add(this.txtId);
            this.panelSlideshow.Controls.Add(this.btnRender);
            this.panelSlideshow.Controls.Add(this.label3);
            this.panelSlideshow.Controls.Add(this.btnDelete);
            this.panelSlideshow.Controls.Add(this.label1);
            this.panelSlideshow.Controls.Add(this.lstArticles);
            this.panelSlideshow.Controls.Add(this.txtTitle);
            this.panelSlideshow.Controls.Add(this.btnAddArticle);
            this.panelSlideshow.Controls.Add(this.label2);
            this.panelSlideshow.Location = new System.Drawing.Point(12, 40);
            this.panelSlideshow.Name = "panelSlideshow";
            this.panelSlideshow.Size = new System.Drawing.Size(1126, 616);
            this.panelSlideshow.TabIndex = 8;
            // 
            // btnOutputFolder
            // 
            this.btnOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOutputFolder.Location = new System.Drawing.Point(468, 535);
            this.btnOutputFolder.Name = "btnOutputFolder";
            this.btnOutputFolder.Size = new System.Drawing.Size(140, 48);
            this.btnOutputFolder.TabIndex = 15;
            this.btnOutputFolder.Text = "Open Slideshow Output Folder";
            this.btnOutputFolder.UseVisualStyleBackColor = true;
            this.btnOutputFolder.Click += new System.EventHandler(this.btnOutputFolder_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.AutoSize = true;
            this.btnRefresh.Location = new System.Drawing.Point(893, 155);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(66, 20);
            this.btnRefresh.TabIndex = 14;
            this.btnRefresh.TabStop = true;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnRefresh_LinkClicked);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPlay.Location = new System.Drawing.Point(614, 535);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(118, 48);
            this.btnPlay.TabIndex = 13;
            this.btnPlay.Text = "Play Narration";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenFolder.Location = new System.Drawing.Point(327, 535);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(135, 48);
            this.btnOpenFolder.TabIndex = 12;
            this.btnOpenFolder.Text = "Open Slideshow Data Folder";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // btnSortByDate
            // 
            this.btnSortByDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSortByDate.Location = new System.Drawing.Point(132, 535);
            this.btnSortByDate.Name = "btnSortByDate";
            this.btnSortByDate.Size = new System.Drawing.Size(118, 48);
            this.btnSortByDate.TabIndex = 11;
            this.btnSortByDate.Text = "Order By Date";
            this.btnSortByDate.UseVisualStyleBackColor = true;
            this.btnSortByDate.Click += new System.EventHandler(this.btnSortByDate_Click);
            // 
            // btnGenerateNarrations
            // 
            this.btnGenerateNarrations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGenerateNarrations.Location = new System.Drawing.Point(8, 535);
            this.btnGenerateNarrations.Name = "btnGenerateNarrations";
            this.btnGenerateNarrations.Size = new System.Drawing.Size(118, 48);
            this.btnGenerateNarrations.TabIndex = 10;
            this.btnGenerateNarrations.Text = "Generate All Narrations";
            this.btnGenerateNarrations.UseVisualStyleBackColor = true;
            this.btnGenerateNarrations.Click += new System.EventHandler(this.btnGenerateNarrations_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(132, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(412, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Ideal letters per image to avoid recycling images: 120-160";
            // 
            // lblTotals
            // 
            this.lblTotals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotals.AutoSize = true;
            this.lblTotals.Location = new System.Drawing.Point(855, 586);
            this.lblTotals.Name = "lblTotals";
            this.lblTotals.Size = new System.Drawing.Size(266, 20);
            this.lblTotals.TabIndex = 9;
            this.lblTotals.Text = "Duration: 00:00:00 Characters: 1000";
            this.lblTotals.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmSlideshow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 668);
            this.Controls.Add(this.panelSlideshow);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1160, 666);
            this.Name = "FrmSlideshow";
            this.Text = "Video From Article";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSlideshow_FormClosing);
            this.Load += new System.EventHandler(this.frmSlideshow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSlideshow_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelSlideshow.ResumeLayout(false);
            this.panelSlideshow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel btnAddArticle;
        private System.Windows.Forms.ListBox lstArticles;
        private System.Windows.Forms.LinkLabel btnDelete;
        private System.Windows.Forms.Button btnRender;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelSlideshow;
        private System.Windows.Forms.Label lblTotals;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGenerateNarrations;
        private System.Windows.Forms.Button btnSortByDate;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.LinkLabel btnRefresh;
        private System.Windows.Forms.Button btnOutputFolder;
    }
}