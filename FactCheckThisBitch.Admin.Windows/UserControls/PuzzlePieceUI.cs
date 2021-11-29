using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FactCheckThisBitch.Models;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class PuzzlePieceUI : UserControl
    {
        private Piece _piece;
        public Piece Piece
        {
            get => _piece;
            set
            {
                _piece = value;
                Load();
            }
        }

        public Action OnClick;
        public Action<string, string> OnDragDrop;

        public PuzzlePieceUI()
        {
            InitializeComponent();
        }

        private void Load()
        {
            lblType.Text = _piece.Type.ToString();
            btnLabel.Text = _piece.Title;
            lblKeywords.Text = string.Join(Environment.NewLine, _piece.Keywords);
        }

        private void btnLabel_Click(object sender, EventArgs e)
        {
            OnClick?.Invoke();
        }

        private void lblType_MouseDown(object sender, MouseEventArgs e)
        {
            lblType.DoDragDrop(_piece, DragDropEffects.All);
        }

        private void lblType_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void lblType_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(Piece)) as Piece;
            OnDragDrop?.Invoke(data.Id, _piece.Id);
        }
    }
}
