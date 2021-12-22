using FactCheckThisBitch.Models;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.Forms
{
    public partial class FrmImageEdit : Form
    {
        public ImageEdit _imageEdit;

        bool dragging = false;
        Rectangle selectedRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));
        Point dragStartPoint;

        public FrmImageEdit(ImageEdit imageEdit)
        {
            InitializeComponent();

            _imageEdit = imageEdit;
            InitForm();
            LoadForm();
        }

        private void InitForm()
        {
        }

        private void LoadForm()
        {
            Text = _imageEdit.Image;
            pictureBox1.ImageLocation = Path.Combine(Configuration.Instance().DataFolder, "media", _imageEdit.Image);
            listBox1.DataSource = _imageEdit.BlurryAreas.Select(b => b.ToCoordinates()).ToList();

            DrawRectangles();
        }

        private void SaveForm()
        {
        }

        private void DrawRectangles()
        {
            foreach (var rectangle in _imageEdit.BlurryAreas)
            {
                DrawRectangle(rectangle);
            }
        }

        private void DrawRectangle(Rectangle rectangle)
        {
            ControlPaint.DrawReversibleFrame(rectangle.RectangleToScreen(pictureBox1), Color.Black, FrameStyle.Thick);
        }

        #region events

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveForm();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
            }

            Control control = (Control) sender;
            dragStartPoint = new Point(e.X, e.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                ControlPaint.DrawReversibleFrame(selectedRectangle.RectangleToScreen(sender as Control), this.BackColor, FrameStyle.Dashed);
                Point endPoint = new Point(e.X, e.Y);

                var endPointX = endPoint.X <= pictureBox1.Width ? endPoint.X : pictureBox1.Width;
                var endPointY = endPoint.Y <= pictureBox1.Height ? endPoint.Y : pictureBox1.Height;

                int width = endPointX - dragStartPoint.X;
                int height = endPointY - dragStartPoint.Y;

                selectedRectangle = new Rectangle(dragStartPoint.X, dragStartPoint.Y, width, height);
                ControlPaint.DrawReversibleFrame(selectedRectangle.RectangleToScreen(sender as Control), this.BackColor, FrameStyle.Dashed);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            ControlPaint.DrawReversibleFrame(selectedRectangle.RectangleToScreen(sender as Control), this.BackColor, FrameStyle.Dashed);

            _imageEdit.BlurryAreas.Add(selectedRectangle);
            LoadForm();
            selectedRectangle = new Rectangle(0, 0, 0, 0);
        }

        private void btnDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            _imageEdit.BlurryAreas.Remove(
                _imageEdit.BlurryAreas.First(b => b.ToCoordinates() == listBox1.SelectedItem.ToString()));
            LoadForm();
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var blurryArea = _imageEdit.BlurryAreas.First(b => b.ToCoordinates() == listBox1.SelectedItem.ToString());
            DrawRectangle(blurryArea);
        }

        #endregion
    }
}