using SixLabors.ImageSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FackCheckThisBitch.Common;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Processing;
using FactCheckThisBitch.Models;
using SixLabors.ImageSharp.Drawing.Processing;
using Syncfusion.Presentation;
using Syncfusion.Presentation.SlideTransition;

namespace FactCheckThisBitch.Render
{
    public class PuzzleRenderer : IDisposable
    {
        private readonly Puzzle _puzzle;
        private readonly string _assetsFolder;
        private readonly string _outputFolder;

        public PuzzleRenderer(Puzzle puzzle, string assetsFolder, string outputFolder)
        {
            _puzzle = puzzle;
            _assetsFolder = assetsFolder;
            _outputFolder = outputFolder;
        }

        public void Render()
        {
            PrepareOutputFolder();
            RenderPiecesWithKeywords();
            RenderPuzzleForEachSlide();
            CreatePresentationFromTemplate();
            OpenOutputFolder();
        }

        private void OpenOutputFolder()
        {
            new Process
            {
                StartInfo = new ProcessStartInfo(_outputFolder)
                {
                    UseShellExecute = true
                }
            }.Start();
        }

        private void PrepareOutputFolder()
        {
            if (Directory.Exists(_outputFolder))
            {
                Directory.Delete(_outputFolder, true);
            }

            Directory.CreateDirectory(_outputFolder);
        }

        private void RenderPuzzleForEachSlide()
        {
            var emptyPuzzleImagePath = Path.Combine(_assetsFolder, $"Puzzle0.png");
            File.Copy(emptyPuzzleImagePath, Path.Combine(_outputFolder, "Puzzle0.png"));
            foreach (var puzzlePiece in _puzzle.PuzzlePieces)
            {
                var slideIndex = puzzlePiece.Index;
                var puzzleForPreviousSlidePath = slideIndex == 1
                    ? emptyPuzzleImagePath
                    : Path.Combine(_outputFolder, $"Puzzle{slideIndex - 1}.png");
                var pieceImageWithKeywordsPath = Path.Combine(_outputFolder, $"Piece{puzzlePiece.Index}_Keywords.png");
                using (var basePuzzle = Image.Load(puzzleForPreviousSlidePath))
                {
                    using (var puzzleForThisSlide = basePuzzle.Clone(ctx =>
                    {
                        using (var pieceWithKeywordsImage = Image.Load(pieceImageWithKeywordsPath))
                        {
                            var location = _puzzle.GetPieceCoordinates(puzzlePiece.Index);
                            ctx.DrawImage(pieceWithKeywordsImage, new Point(location.X, location.Y), 1);
                        }
                    }))
                    {
                        puzzleForThisSlide.Save(Path.Combine(_outputFolder, $"Puzzle{slideIndex}.png"));
                    }
                }
            }
        }

        private void RenderPiecesWithKeywords()
        {
            foreach (var puzzlePiece in _puzzle.PuzzlePieces)
            {
                var pieceImagePath = Path.Combine(_assetsFolder, $"Piece{puzzlePiece.Index}.png");
                var pieceImgeWithKeywordsPath = Path.Combine(_outputFolder, $"Piece{puzzlePiece.Index}_Keywords.png");
                using (var pieceImage = Image.Load(pieceImagePath))
                {
                    var pieceCoordinates = Extensions.GetPieceCoordinates(_puzzle, puzzlePiece.Index);
                    Font font = SystemFonts.CreateFont("Arial", 10); // for scaling water mark size is largely ignored.
                    using (var pieceImageWithKeywords = pieceImage.Clone(ctx =>
                    {
                        ImageSharpExtensions.ApplyScalingWaterMark(ctx, font, puzzlePiece.Piece.GetKeywordsText(),
                            pieceCoordinates.KeywordsBox, Color.White, 5, false);
                    }))
                    {
                        pieceImageWithKeywords.Save(pieceImgeWithKeywordsPath);
                    }
                }
            }
        }

        private void CreatePresentationFromTemplate()
        {
            var templatePath = Path.Combine(_assetsFolder, "Template.pptx");
            IPresentation doc = Presentation.Open(templatePath);

            #region introduction slide
            var puzzleSlide = doc.Slides[0];
            puzzleSlide.GroupShape("puzzle_metadata").UpdateText("puzzle_title", _puzzle.Title);
            puzzleSlide.GroupShape("puzzle_metadata").UpdateText("puzzle_thesis", _puzzle.Thesis);
            #endregion

            #region create one slide for each piece

            var pieceSlideTemplate = doc.Slides[1];
            foreach (var puzzlePiece in _puzzle.PuzzlePieces)
            {
                var newPieceSlide = pieceSlideTemplate.Clone();
                newPieceSlide.SlideTransition.TransitionEffect = TransitionEffect.None;

                newPieceSlide.GroupShape("piece_metadata").UpdateText("piece_title", puzzlePiece.Piece.Title);
                newPieceSlide.GroupShape("piece_metadata").UpdateText("piece_thesis", puzzlePiece.Piece.Thesis);

                //replace puzzle picture
                newPieceSlide.ReplacePicture("empty_puzzle",Path.Combine(_outputFolder,$"Puzzle{puzzlePiece.Index-1}.png"));

                //replace piece picture
                newPieceSlide.ReplacePicture("puzzle_piece",Path.Combine(_outputFolder,$"Piece{puzzlePiece.Index}_Keywords.png"));

                doc.Slides.Add(newPieceSlide);
            }

            #endregion

            doc.Slides.RemoveAt(1);

            doc.Save(Path.Combine(_outputFolder, $"{_puzzle.Title.ToSanitizedString()}.pptx"));
        }

        public void Dispose()
        {
        }
    }
}