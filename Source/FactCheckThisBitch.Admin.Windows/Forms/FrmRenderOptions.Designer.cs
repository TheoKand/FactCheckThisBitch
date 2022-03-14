
namespace FactCheckThisBitch.Admin.Windows.Forms
{
    partial class FrmRenderOptions
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
            this.label1 = new System.Windows.Forms.Label();
            this.lstTemplate = new System.Windows.Forms.ListBox();
            this.chkWrongSpeak = new System.Windows.Forms.CheckBox();
            this.chkBlurryAreas = new System.Windows.Forms.CheckBox();
            this.chkRealAiNews = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(84, 235);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(118, 40);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(207, 235);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(118, 40);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Template:";
            // 
            // lstTemplate
            // 
            this.lstTemplate.FormattingEnabled = true;
            this.lstTemplate.ItemHeight = 17;
            this.lstTemplate.Location = new System.Drawing.Point(10, 38);
            this.lstTemplate.Name = "lstTemplate";
            this.lstTemplate.Size = new System.Drawing.Size(323, 89);
            this.lstTemplate.TabIndex = 6;
            // 
            // chkWrongSpeak
            // 
            this.chkWrongSpeak.AutoSize = true;
            this.chkWrongSpeak.Location = new System.Drawing.Point(10, 165);
            this.chkWrongSpeak.Name = "chkWrongSpeak";
            this.chkWrongSpeak.Size = new System.Drawing.Size(147, 21);
            this.chkWrongSpeak.TabIndex = 7;
            this.chkWrongSpeak.Text = "Handle WrongSpeak";
            this.chkWrongSpeak.UseVisualStyleBackColor = true;
            // 
            // chkBlurryAreas
            // 
            this.chkBlurryAreas.AutoSize = true;
            this.chkBlurryAreas.Location = new System.Drawing.Point(10, 193);
            this.chkBlurryAreas.Name = "chkBlurryAreas";
            this.chkBlurryAreas.Size = new System.Drawing.Size(142, 21);
            this.chkBlurryAreas.TabIndex = 8;
            this.chkBlurryAreas.Text = "Handle Blurry Areas";
            this.chkBlurryAreas.UseVisualStyleBackColor = true;
            // 
            // chkRealAiNews
            // 
            this.chkRealAiNews.AutoSize = true;
            this.chkRealAiNews.Location = new System.Drawing.Point(10, 138);
            this.chkRealAiNews.Name = "chkRealAiNews";
            this.chkRealAiNews.Size = new System.Drawing.Size(165, 21);
            this.chkRealAiNews.TabIndex = 9;
            this.chkRealAiNews.Text = "Render for RealAi.News";
            this.chkRealAiNews.UseVisualStyleBackColor = true;
            // 
            // FrmRenderOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(344, 295);
            this.Controls.Add(this.chkRealAiNews);
            this.Controls.Add(this.chkBlurryAreas);
            this.Controls.Add(this.chkWrongSpeak);
            this.Controls.Add(this.lstTemplate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRenderOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstTemplate;
        private System.Windows.Forms.CheckBox chkWrongSpeak;
        private System.Windows.Forms.CheckBox chkBlurryAreas;
        private System.Windows.Forms.CheckBox chkRealAiNews;
    }
}