using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FactCheckThisBitch.Admin.Windows.Forms;
using FactCheckThisBitch.Models;

namespace FactCheckThisBitch.Admin.Windows
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FrmPuzzle puzzleForm = new FrmPuzzle(new Puzzle()
            {
                Title = "Are Covid Vaccines Really Vaccines?",
                Thesis = "hah ahahahaha",
                Width = 4,
                Height = 4
            });
            puzzleForm.Show();

            //FrmPuzzle puzzleForm = new FrmPuzzle( null);
            //puzzleForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {


            var piece = new Piece
            {
                Type = PieceType.Article,
                Title = "Are Covid Vaccines Really Vaccines?",
                Thesis = "hah ahahahaha",
                Keywords = new[] {"Vaccine", "Bullshit"},
                Content = new Article
                {
                    Title = "GMOs: Transgenic Crops and Recombinant DNA Technology",
                    Summary =
                        "If you could save lives by producing vaccines in transgenic bananas, would you? In the debate over large-scale commercialization and use of GMOs, where should we draw the line?",
                    DatePublished = DateTime.Parse("25/01/2021"),
                    Source = "Sovereign Wealth Fund Institute",
                    Url = new Uri(
                        "https://www.nature.com/scitable/topicpage/genetically-modified-organisms-gmos-transgenic-crops-and-732/"),
                    References = new[] {"one", "two"}
                }
            };
            FrmPiece pieceForm = new FrmPiece(piece);
            pieceForm.Show();
        }
    }
}
