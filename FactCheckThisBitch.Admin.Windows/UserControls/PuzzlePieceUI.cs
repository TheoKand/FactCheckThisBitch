using FactCheckThisBitch.Models;
using System;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class PuzzlePieceUi : UserControl
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

        public new Action OnClick;
        public new Action<string, string> OnDragDrop;

        public PuzzlePieceUi()
        {
            InitializeComponent();
        }

        private new void Load()
        {
            lblType.Text = _piece.Type.ToString();
            btnLabel.Text = _piece.Title;
            lblKeywords.Text = string.Join(Environment.NewLine, _piece.Keywords);
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
            OnDragDrop?.Invoke(data?.Id, _piece.Id);
        }

        private void btnLabel_Click_1(object sender, EventArgs e)
        {
            OnClick?.Invoke();
        }
    }
}