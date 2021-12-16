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

        public PuzzleRenderer(Puzzle puzzle, string assetsFolder, string outputFolder, string mediaFolder)
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
            FixSlidesDuration();
            OpenOutputFolder();
        }

        private void OpenOutputFolder()
        {
            new Process {StartInfo = new ProcessStartInfo(_outputFolder) {UseShellExecute = true}}.Start();
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
                        var keywordsBoxes = pieceCoordinates.KeywordsBoxes as dynamic[];
                        if (keywordsBoxes.Length > 1 && puzzlePiece.Piece.Keywords.Count>1)
                        {
                            ImageSharpExtensions.ApplyScalingWaterMark(ctx, font, puzzlePiece.Piece.GetKeywordsText(0,1),
                                keywordsBoxes[0], Color.White, 5, false);

                            ImageSharpExtensions.ApplyScalingWaterMark(ctx, font, puzzlePiece.Piece.GetKeywordsText(1),
                                keywordsBoxes[1], Color.White, 5, false);
                        }
                        else
                        {
                            ImageSharpExtensions.ApplyScalingWaterMark(ctx, font, puzzlePiece.Piece.GetKeywordsText(),
                                keywordsBoxes[0], Color.White, 5, false);
                        }

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
            using (IPresentation doc = Presentation.Open(templatePath))
            {
                #region introduction slide

                var puzzleSlide = doc.Slides[0];
                puzzleSlide.GroupShape("puzzle_metadata").UpdateText("puzzle_title", _puzzle.Title);
                puzzleSlide.GroupShape("puzzle_metadata").UpdateText("puzzle_thesis", _puzzle.Thesis);

                #endregion

                var pieceSlideTemplate = doc.Slides[1];
                var pieceReferencesTemplate = doc.Slides[2];
                var thinkingSlideTemplate = doc.Slides[3];
                var conclusionSlideTemplate = doc.Slides[4];
                var thinkingSlide = thinkingSlideTemplate.Clone();
                var conclusionSlide = conclusionSlideTemplate.Clone();

                #region create one slide for each piece

                foreach (var puzzlePiece in _puzzle.PuzzlePieces)
                {
                    var newPieceSlide = pieceSlideTemplate.Clone();
                    newPieceSlide.Name = puzzlePiece.Piece.Id;
                    newPieceSlide.SlideTransition.TransitionEffect = TransitionEffect.None;
                    newPieceSlide.GroupShape("piece_metadata").UpdateText("piece_title", puzzlePiece.Piece.Title);
                    newPieceSlide.GroupShape("piece_metadata").UpdateText("piece_thesis", puzzlePiece.Piece.Thesis);

                    //replace puzzle picture
                    newPieceSlide.ReplacePicture("empty_puzzle",
                        Path.Combine(_outputFolder, $"Puzzle{puzzlePiece.Index - 1}.png"));

                    //replace piece picture
                    newPieceSlide.ReplacePicture("puzzle_piece",
                        Path.Combine(_outputFolder, $"Piece{puzzlePiece.Index}_Keywords.png"));

                    #region position puzzle piece inside puzzle

                    var pieceCoordinates = _puzzle.GetPieceCoordinates(puzzlePiece.Index);
                    var puzzlePicture = newPieceSlide.Shapes.First(p => p.ShapeName == "empty_puzzle");
                    var puzzlePieceShape = newPieceSlide.Shapes.First(s => s.ShapeName == "puzzle_piece");
                    double scaleRatioOfPuzzle = 0.88;
                    var pieceLeft = puzzlePicture.Left + ((double) pieceCoordinates.X * scaleRatioOfPuzzle).PixelsToPoints();
                    var pieceTop = puzzlePicture.Top + ((double) pieceCoordinates.Y * scaleRatioOfPuzzle).PixelsToPoints();
                    using (var pieceImage = Image.Load(Path.Combine(_outputFolder, $"Piece{puzzlePiece.Index}_Keywords.png")))
                    {
                        var pieceImageWidth = ((double) pieceImage.Width * scaleRatioOfPuzzle).PixelsToPoints();
                        var pieceImageHeight = ((double) pieceImage.Height * scaleRatioOfPuzzle).PixelsToPoints();
                        puzzlePieceShape.Left = pieceLeft;
                        puzzlePieceShape.Top = pieceTop;
                        puzzlePieceShape.Width = pieceImageWidth;
                        puzzlePieceShape.Height = pieceImageHeight;
                    }

                    #endregion

                    doc.Slides.Add(newPieceSlide);

                    #region Create a slide for each reference of this piece

                    var referenceIndex = 0;
                    foreach (var reference in puzzlePiece.Piece.References)
                    {
                        foreach (string image in reference.Images)
                        {
                            var referenceSlide = pieceReferencesTemplate.Clone();
                            referenceSlide.Name = $"{reference.Id}-{image}";
                            var slideSequence = referenceSlide.Timeline.MainSequence;
                            var referenceGroupShape = referenceSlide.GroupShape("group_article");
                            referenceGroupShape.UpdateText("textbox_url", reference.Url.Limit(100));
                            referenceGroupShape.UpdateText("textbox_source",
                                reference.Source.IsEmpty() ? "" : $"Source: {reference.Source}");
                            referenceGroupShape.UpdateText("textbox_date",
                                (!reference.DatePublished.HasValue)
                                    ? ""
                                    : $"Date Published: {reference.DatePublished.Value:dd/MM/yyyy}");
                            referenceGroupShape.ReplacePicture("picture_screenshot", Path.Combine(_mediaFolder, image));
                            if (referenceIndex > 0)
                            {
                                slideSequence.RemoveByShape(referenceGroupShape as IShape);
                                referenceSlide.Pictures.RemoveAt(0);
                            }

                            doc.Slides.Add(referenceSlide);
                            referenceIndex++;
                        }
                    }

                    #endregion
                }

                #endregion

                doc.Slides.Remove(pieceSlideTemplate);
                doc.Slides.Remove(pieceReferencesTemplate);
                doc.Slides.Remove(thinkingSlideTemplate);
                doc.Slides.Remove(conclusionSlideTemplate);

                #region conclusion slide
                conclusionSlide.UpdateText("conclusion_text",_puzzle.Conclusion);
                conclusionSlide.ReplacePicture("conclusion_puzzle",Path.Combine(_outputFolder,"Puzzle9.png"));
                doc.Slides.Add(thinkingSlide);
                doc.Slides.Add(conclusionSlide);

                #endregion

                doc.Save(Path.Combine(_outputFolder, $"{_puzzle.Title.ToSanitizedString()}.pptx"));
            }
        }

        public void FixSlidesDuration()
        {
            var puzzlePresentationPath = Path.Combine(_outputFolder, $"{_puzzle.Title.ToSanitizedString()}.pptx");
            using (IPresentation doc = Presentation.Open(puzzlePresentationPath))
            {
                doc.Slides[0].SlideTransition.TimeDelay = _puzzle.Duration;
                foreach (var puzzlePiece in _puzzle.PuzzlePieces)
                {
                    bool isFirstImageInPiece = true;
                    doc.Slides.First(s => s.Name == puzzlePiece.Piece.Id).SlideTransition.TimeDelay = puzzlePiece.Piece.Duration;
                    foreach (var reference in puzzlePiece.Piece.References)
                    {
                        foreach (var image in reference.Images)
                        {
                            var slide = doc.Slides.First(s => s.Name == $"{reference.Id}-{image}");
                            if (!isFirstImageInPiece)
                            {
                                slide.SlideTransition.TransitionEffect = TransitionEffect.FadeAway;
                                slide.SlideTransition.Duration = 0.4f;
                                slide.SlideTransition.TimeDelay = reference.Duration - 0.5f;
                            }
                            else
                            {
                                slide.SlideTransition.TimeDelay = reference.Duration;
                            }

                            isFirstImageInPiece = false;
                        }
                    }
                }

                doc.Save(puzzlePresentationPath);
            }
        }

        public void Dispose()
        {
        }
    }
}