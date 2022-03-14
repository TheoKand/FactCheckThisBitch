
namespace VideoFromArticle.Admin.Windows.Forms
{
    partial class FrmArticle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmArticle));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnUrl = new System.Windows.Forms.LinkLabel();
            this.btnDownload = new System.Windows.Forms.Button();
            this.imageEditor1 = new FactCheckThisBitch.Admin.Windows.UserControls.ImageEditor();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDatePublished = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNarration = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOpenAllImages = new System.Windows.Forms.LinkLabel();
            this.btnGenerateAudio = new System.Windows.Forms.LinkLabel();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(895, 696);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(212, 64);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(678, 696);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(212, 64);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Ok";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(81, 12);
            this.txtTitle.Margin = new System.Windows.Forms.Padding(4);
            this.txtTitle.Multiline = true;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTitle.Size = new System.Drawing.Size(1002, 63);
            this.txtTitle.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(17, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Title";
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(81, 83);
            this.txtUrl.Margin = new System.Windows.Forms.Padding(4);
            this.txtUrl.Multiline = true;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUrl.Size = new System.Drawing.Size(1002, 46);
            this.txtUrl.TabIndex = 1;
            // 
            // btnUrl
            // 
            this.btnUrl.AutoSize = true;
            this.btnUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnUrl.Location = new System.Drawing.Point(17, 84);
            this.btnUrl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnUrl.Name = "btnUrl";
            this.btnUrl.Size = new System.Drawing.Size(29, 20);
            this.btnUrl.TabIndex = 14;
            this.btnUrl.TabStop = true;
            this.btnUrl.Text = "Url";
            this.btnUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnUrl_LinkClicked);
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDownload.Location = new System.Drawing.Point(17, 696);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(4);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(105, 64);
            this.btnDownload.TabIndex = 6;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // imageEditor1
            // 
            this.imageEditor1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageEditor1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imageEditor1.Images = ((System.Collections.Generic.List<string>)(resources.GetObject("imageEditor1.Images")));
            this.imageEditor1.Location = new System.Drawing.Point(13, 172);
            this.imageEditor1.Margin = new System.Windows.Forms.Padding(4);
            this.imageEditor1.Name = "imageEditor1";
            this.imageEditor1.Size = new System.Drawing.Size(1089, 195);
            this.imageEditor1.TabIndex = 4;
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(82, 138);
            this.txtSource.Margin = new System.Windows.Forms.Padding(4);
            this.txtSource.Name = "txtSource";
            this.txtSource.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSource.Size = new System.Drawing.Size(305, 26);
            this.txtSource.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(18, 142);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 20);
            this.label2.TabIndex = 18;
            this.label2.Text = "Source";
            // 
            // txtDatePublished
            // 
            this.txtDatePublished.Location = new System.Drawing.Point(513, 138);
            this.txtDatePublished.Margin = new System.Windows.Forms.Padding(4);
            this.txtDatePublished.Name = "txtDatePublished";
            this.txtDatePublished.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDatePublished.Size = new System.Drawing.Size(154, 26);
            this.txtDatePublished.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(395, 145);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 20);
            this.label3.TabIndex = 20;
            this.label3.Text = "Date Published";
            // 
            // txtNarration
            // 
            this.txtNarration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNarration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNarration.Location = new System.Drawing.Point(13, 403);
            this.txtNarration.Margin = new System.Windows.Forms.Padding(4);
            this.txtNarration.Multiline = true;
            this.txtNarration.Name = "txtNarration";
            this.txtNarration.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNarration.Size = new System.Drawing.Size(1088, 276);
            this.txtNarration.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(13, 379);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 20);
            this.label4.TabIndex = 22;
            this.label4.Text = "Narration";
            // 
            // btnOpenAllImages
            // 
            this.btnOpenAllImages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenAllImages.AutoSize = true;
            this.btnOpenAllImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenAllImages.Location = new System.Drawing.Point(976, 148);
            this.btnOpenAllImages.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnOpenAllImages.Name = "btnOpenAllImages";
            this.btnOpenAllImages.Size = new System.Drawing.Size(126, 20);
            this.btnOpenAllImages.TabIndex = 23;
            this.btnOpenAllImages.TabStop = true;
            this.btnOpenAllImages.Text = "Open All Images";
            this.btnOpenAllImages.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnOpenAllImages_LinkClicked);
            // 
            // btnGenerateAudio
            // 
            this.btnGenerateAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateAudio.AutoSize = true;
            this.btnGenerateAudio.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnGenerateAudio.Location = new System.Drawing.Point(916, 379);
            this.btnGenerateAudio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnGenerateAudio.Name = "btnGenerateAudio";
            this.btnGenerateAudio.Size = new System.Drawing.Size(191, 20);
            this.btnGenerateAudio.TabIndex = 24;
            this.btnGenerateAudio.TabStop = true;
            this.btnGenerateAudio.Text = "Generate Narration Audio";
            this.btnGenerateAudio.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnGenerateAudio_LinkClicked);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenFolder.Location = new System.Drawing.Point(130, 696);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(105, 64);
            this.btnOpenFolder.TabIndex = 25;
            this.btnOpenFolder.Text = "Open Folder";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // FrmArticle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1121, 774);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.btnGenerateAudio);
            this.Controls.Add(this.btnOpenAllImages);
            this.Controls.Add(this.txtNarration);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDatePublished);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.imageEditor1);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnUrl);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(859, 726);
            this.Name = "FrmArticle";
            this.Text = "SlideshowArticle";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmArticle_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.LinkLabel btnUrl;
        private System.Windows.Forms.Button btnDownload;
        private FactCheckThisBitch.Admin.Windows.UserControls.ImageEditor imageEditor1;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDatePublished;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNarration;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel btnOpenAllImages;
        private System.Windows.Forms.LinkLabel btnGenerateAudio;
        private System.Windows.Forms.Button btnOpenFolder;
    }
}