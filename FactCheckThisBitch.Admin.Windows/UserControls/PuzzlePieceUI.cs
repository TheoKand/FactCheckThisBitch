using FactCheckThisBitch.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows.UserControls
{
    public partial class PuzzlePieceUi : UserControl
    {
        private PuzzlePiece _puzzlePiece;

        public PuzzlePiece PuzzlePiece
        {
            get => _puzzlePiece;
            set
            {
                _puzzlePiece = value;
                Load();
            }
        }

        public new Action OnClick;
        public new Action<int, int> OnDragDrop;

        public bool ConnectedTop
        {
            get => dotTop.Visible;
            set => dotTop.Visible = value;
        }

        public bool ConnectedLeft
        {
            get => dotLeft.Visible;
            set => dotLeft.Visible = value;
        }

        public bool ConnectedRight
        {
            get => dotRight.Visible;
            set => dotRight.Visible = value;
        }

        public bool ConnectedBottom
        {
            get => dotBottom.Visible;
            set => dotBottom.Visible = value;
        }

        public PuzzlePieceUi()
        {
            InitializeComponent();
        }

        private new void Load()
        {
            btnLabel.Text = _puzzlePiece.Piece.Title;
            lblKeywords.Text = _puzzlePiece.Piece.Keywords != null ? string.Join(Environment.NewLine, _puzzlePiece.Piece.Keywords) : "";
        }

        #region events
        private void lblType_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                lblType.DoDragDrop(_puzzlePiece.Index, DragDropEffects.All);
            }
        }

        private void lblType_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void lblType_DragDrop(object sender, DragEventArgs e)
        {
            var draggedPieceIndexs = (int)e.Data.GetData(typeof(int));
            OnDragDrop?.Invoke(draggedPieceIndexs, _puzzlePiece.Index);
        }

        private void btnLabel_Click_1(object sender, EventArgs e)
        {
            OnClick?.Invoke();
        }
        #endregion
    }
}