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
        private readonly string _mediaFolder;

        public PuzzleRenderer(Puzzle puzzle, string assetsFolder, string outputFolder,string mediaFolder)
        {
            _puzzle = puzzle;
            _assetsFolder = assetsFolder;
            _outputFolder = outputFolder;
            _mediaFolder = mediaFolder;
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
            puzzleSlide.SlideTransition.TimeDelay = _puzzle.Duration;
        
            #endregion

            var pieceSlideTemplate = doc.Slides[1];
            var pieceReferencesTemplate = doc.Slides[2];


            #region create one slide for each piece
            foreach (var puzzlePiece in _puzzle.PuzzlePieces)
            {
                var newPieceSlide = pieceSlideTemplate.Clone();
                newPieceSlide.SlideTransition.TransitionEffect = TransitionEffect.None;
                newPieceSlide.SlideTransition.TimeDelay = puzzlePiece.Piece.Duration;

                newPieceSlide.GroupShape("piece_metadata").UpdateText("piece_title", puzzlePiece.Piece.Title);
                newPieceSlide.GroupShape("piece_metadata").UpdateText("piece_thesis", puzzlePiece.Piece.Thesis);

                //replace puzzle picture
                newPieceSlide.ReplacePicture("empty_puzzle",Path.Combine(_outputFolder,$"Puzzle{puzzlePiece.Index-1}.png"));

                //replace piece picture
                newPieceSlide.ReplacePicture("puzzle_piece",Path.Combine(_outputFolder,$"Piece{puzzlePiece.Index}_Keywords.png"));

                //position piece picture
                //get location of puzzle picture
                var location = _puzzle.GetPieceCoordinates(puzzlePiece.Index);
                var puzzlePicture = newPieceSlide.Shapes.First(p => p.ShapeName == "empty_puzzle");
                var puzzlePieceShape = newPieceSlide.Shapes.First(s => s.ShapeName == "puzzle_piece");

                var xRatio =0.68496500437;
                var yRatio = 0.68218519437;

                var pieceImageWidth = 0d;
                var pieceImageHeight = 0d;
                var pieceImagePath = Path.Combine(_outputFolder, $"Piece{puzzlePiece.Index}_Keywords.png");
                using (var pieceImage = Image.Load(pieceImagePath))
                {
                    pieceImageWidth = (double)pieceImage.Width * xRatio ;
                    pieceImageHeight = (double)pieceImage.Height* yRatio;
                }

                puzzlePieceShape.Left = puzzlePicture.Left + location.X* xRatio;
                puzzlePieceShape.Top = puzzlePicture.Top + location.Y * yRatio;
                puzzlePieceShape.Width = pieceImageWidth;
                puzzlePieceShape.Height = pieceImageHeight;

                doc.Slides.Add(newPieceSlide);

                #region Create a slide for each reference of this piece


                var referenceIndex = 0;
                foreach (var reference in puzzlePiece.Piece.References)
                {


                    foreach (string image in reference.Images)
                    {

                        var referenceSlide = pieceReferencesTemplate.Clone();
                        var slideSequence = referenceSlide.Timeline.MainSequence;

                        var referenceGroupShape = referenceSlide.GroupShape("group_article");
                        referenceGroupShape.UpdateText("textbox_url", reference.Url.Limit(100));
                        referenceGroupShape.ReplacePicture("picture_screenshot",
                            Path.Combine(_mediaFolder, image)); //todo: handle multiple images

                        //todo: if it's not the first reference of the piece, remove the tv picture and set
                        //the slide transition to fade
                        if (referenceIndex > 0)
                        {
                            slideSequence.RemoveByShape(referenceGroupShape as IShape);

                            referenceSlide.SlideTransition.TransitionEffect = TransitionEffect.None; //TODO: fix
                            referenceSlide.Pictures.RemoveAt(0);
                        }
                        else
                        {

                        }

                        referenceSlide.SlideTransition.TimeDelay = reference.Duration;
                        doc.Slides.Add(referenceSlide);

                        referenceIndex++;

                    }


                }


                #endregion
            }

            #endregion

            doc.Slides.Remove(pieceSlideTemplate);
            doc.Slides.Remove(pieceReferencesTemplate);

            doc.Save(Path.Combine(_outputFolder, $"{_puzzle.Title.ToSanitizedString()}.pptx"));
        }

        public void Dispose()
        {
        }
    }
}