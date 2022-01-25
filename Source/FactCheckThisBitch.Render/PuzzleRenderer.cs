using FackCheckThisBitch.Common;
using FactCheckThisBitch.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Syncfusion.Presentation;
using Syncfusion.Presentation.SlideTransition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FactCheckThisBitch.Render
{
    public class PuzzleRenderer : IDisposable
    {
        private readonly Puzzle _puzzle;
        private readonly string _template;
        private readonly string _assetsFolder;
        private readonly string _outputFolder;
        private readonly string _mediaFolder;
        private readonly bool _handleWrongSpeak;
        private readonly bool _handleBlurryAreas;
        private readonly bool _realAiNewsRender;

        private readonly PuzzleTheme _puzzleTheme = new PuzzleTheme
        {
            PieceBgColor = PuzzleTheme.PieceBGColors.Blue,
            PieceTextColor = Color.White
        };

        public PuzzleRenderer(Puzzle puzzle, string template, string assetsFolder, string outputFolder, string mediaFolder, bool realAiNewsRender, bool handleWrongSpeak = false, bool handleBlurryAreas = false)
        {
            _puzzle = puzzle;
            _template = template;
            _assetsFolder = assetsFolder;
            _outputFolder = outputFolder;
            _mediaFolder = mediaFolder;
            _handleWrongSpeak = handleWrongSpeak;
            _handleBlurryAreas = handleBlurryAreas;
            _realAiNewsRender = realAiNewsRender;
        }

        public void Render()
        {
            PrepareOutputFolder();
            RenderPiecesWithKeywords();
            RenderPuzzleForEachSlide();
            CreatePresentationFromTemplate();
            FixSlidesDuration();
            GenerateNarrationForSpeechelo();
            OpenOutputFolder();
        }

        private void OpenOutputFolder()
        {
            new Process { StartInfo = new ProcessStartInfo(_outputFolder) { UseShellExecute = true } }.Start();
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
            var emptyPuzzleImagePath = Path.Combine(_assetsFolder, "Images", "Puzzle", _puzzleTheme.PieceBgColor.ToString(), "Puzzle0.png");
            File.Copy(emptyPuzzleImagePath, Path.Combine(_outputFolder, "Puzzle0.png"));
            foreach (var puzzlePiece in _puzzle.PuzzlePieces.OrderBy(_ => _.RenderOrder))
            {
                var pieceIndex = _puzzle.PuzzlePieces.IndexOf(puzzlePiece) + 1;

                var puzzleForPreviousSlidePath = puzzlePiece.RenderOrder == 1
                    ? emptyPuzzleImagePath
                    : Path.Combine(_outputFolder, $"Puzzle{puzzlePiece.RenderOrder - 1}.png");
                var pieceImageWithKeywordsPath = Path.Combine(_outputFolder, $"Piece{puzzlePiece.RenderOrder}_Keywords.png");
                using var basePuzzle = Image.Load(puzzleForPreviousSlidePath);
                using var puzzleForThisSlide = basePuzzle.Clone(ctx =>
                {
                    using var pieceWithKeywordsImage = Image.Load(pieceImageWithKeywordsPath);
                    var location = _puzzle.GetPieceCoordinates(pieceIndex);
                    ctx.DrawImage(pieceWithKeywordsImage, new Point(location.X, location.Y), 1);
                });
                puzzleForThisSlide.Save(Path.Combine(_outputFolder, $"Puzzle{puzzlePiece.RenderOrder}.png"));
            }
        }

        private void RenderPiecesWithKeywords()
        {
            var leetLevel = _handleWrongSpeak ? Level.Minimum : Level.None;

            foreach (var puzzlePiece in _puzzle.PuzzlePieces.OrderBy(_ => _.RenderOrder))
            {
                if (puzzlePiece.RenderOrder > _puzzle.Width * _puzzle.Height) continue;

                var pieceIndex = _puzzle.PuzzlePieces.IndexOf(puzzlePiece) + 1;

                var pieceTemplatePath = Path.Combine(_assetsFolder, "Images", "Puzzle", _puzzleTheme.PieceBgColor.ToString(), $"Piece{pieceIndex}.png");
                var pieceWithKeywordsPath = Path.Combine(_outputFolder, $"Piece{puzzlePiece.RenderOrder}_Keywords.png");
                using var pieceImage = Image.Load(pieceTemplatePath);
                var pieceCoordinates = Extensions.GetPieceCoordinates(_puzzle, pieceIndex);
                Font font = SystemFonts.CreateFont("Arial", 10); // for scaling water mark size is largely ignored.
                using var pieceImageWithKeywords = pieceImage.Clone(ctx =>
                {
                    var keywordsBoxes = pieceCoordinates.KeywordsBoxes as dynamic[];
                    if (keywordsBoxes.Length > 1 && puzzlePiece.Piece.Keywords.Count > 1)
                    {
                        ImageSharpExtensions.ApplyScalingWaterMark(ctx, font,
                            puzzlePiece.Piece.GetKeywordsText(0, 1).WrongSpeakToLeetSpeak(leetLevel),
                            keywordsBoxes[0], _puzzleTheme.PieceTextColor, 5, false);

                        ImageSharpExtensions.ApplyScalingWaterMark(ctx, font,
                            puzzlePiece.Piece.GetKeywordsText(1).WrongSpeakToLeetSpeak(leetLevel),
                            keywordsBoxes[1], _puzzleTheme.PieceTextColor, 5, false);
                    }
                    else
                    {
                        ImageSharpExtensions.ApplyScalingWaterMark(ctx, font,
                            puzzlePiece.Piece.GetKeywordsText().WrongSpeakToLeetSpeak(leetLevel),
                            keywordsBoxes[0], _puzzleTheme.PieceTextColor, 5, false);
                    }
                });
                pieceImageWithKeywords.Save(pieceWithKeywordsPath);
            }
        }

        private void CreatePresentationFromTemplate()
        {

            var leetLevel = _handleWrongSpeak ? Level.Minimum : Level.None;

            var templatePath = Path.Combine(_assetsFolder, "Powerpoint", _template);
            using IPresentation doc = Presentation.Open(templatePath);

            #region introduction slide

            var puzzleSlide = doc.Slides[0];
            puzzleSlide.GroupShape("puzzle_metadata").UpdateText("puzzle_title", _puzzle.Title.WrongSpeakToLeetSpeak(leetLevel));
            puzzleSlide.GroupShape("puzzle_metadata").UpdateText("puzzle_thesis", _puzzle.Thesis.WrongSpeakToLeetSpeak(leetLevel));

            #endregion

            ISlide pieceSlideTemplate = null;
            ISlide pieceReferencesTemplate = null;
            ISlide thinkingSlideTemplate = null;
            ISlide conclusionSlideTemplate = null;
            ISlide thinkingSlide = null;
            ISlide conclusionSlide = null;

            if (_realAiNewsRender)
            {
                pieceSlideTemplate = doc.Slides[0];
                pieceReferencesTemplate = doc.Slides[1];
            }
            else
            {
                pieceSlideTemplate = doc.Slides[1];
                pieceReferencesTemplate = doc.Slides[2];
                thinkingSlideTemplate = doc.Slides[3];
                conclusionSlideTemplate = doc.Slides[4];
                thinkingSlide = thinkingSlideTemplate.Clone();
                conclusionSlide = conclusionSlideTemplate.Clone();
            }


            #region create one slide for each piece

            foreach (var puzzlePiece in _puzzle.PuzzlePieces.OrderBy(_ => _.RenderOrder))
            {
                var pieceIndex = _puzzle.PuzzlePieces.IndexOf(puzzlePiece) + 1;

                var newPieceSlide = pieceSlideTemplate.Clone();
                newPieceSlide.Name = puzzlePiece.Piece.Id;
                newPieceSlide.SlideTransition.TransitionEffect = TransitionEffect.None;
                newPieceSlide.GroupShape("piece_metadata").UpdateText("piece_title", puzzlePiece.Piece.Title.WrongSpeakToLeetSpeak(leetLevel));
                newPieceSlide.GroupShape("piece_metadata").UpdateText("piece_thesis", puzzlePiece.Piece.Thesis.WrongSpeakToLeetSpeak(leetLevel));

                //replace puzzle picture
                newPieceSlide.ReplacePicture("empty_puzzle",
                    Path.Combine(_outputFolder, $"Puzzle{puzzlePiece.RenderOrder - 1}.png"));

                //replace piece picture
                newPieceSlide.ReplacePicture("puzzle_piece",
                    Path.Combine(_outputFolder, $"Piece{puzzlePiece.RenderOrder}_Keywords.png"));

                #region position puzzle piece inside puzzle

                var pieceCoordinates = _puzzle.GetPieceCoordinates(pieceIndex);
                var puzzlePicture = newPieceSlide.Shapes.First(p => p.ShapeName == "empty_puzzle");
                var puzzlePieceShape = newPieceSlide.Shapes.First(s => s.ShapeName == "puzzle_piece");
                double scaleRatioOfPuzzle = 0.88;
                var pieceLeft = puzzlePicture.Left + ((double)pieceCoordinates.X * scaleRatioOfPuzzle).PixelsToPoints();
                var pieceTop = puzzlePicture.Top + ((double)pieceCoordinates.Y * scaleRatioOfPuzzle).PixelsToPoints();
                using (var pieceImage = Image.Load(Path.Combine(_outputFolder, $"Piece{puzzlePiece.RenderOrder}_Keywords.png")))
                {
                    var pieceImageWidth = (pieceImage.Width * scaleRatioOfPuzzle).PixelsToPoints();
                    var pieceImageHeight = (pieceImage.Height * scaleRatioOfPuzzle).PixelsToPoints();
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
                    foreach (var image in reference.Images)
                    {
                        var referenceSlide = pieceReferencesTemplate.Clone();
                        referenceSlide.Name = $"{reference.Id}-{image}";
                        var slideSequence = referenceSlide.Timeline.MainSequence;
                        var referenceGroupShape = referenceSlide.GroupShape("group_article");
                        referenceGroupShape.UpdateText("textbox_url", reference.Url.Replace("https://", "").Replace("http://", "").Replace("www.", "").Limit(69));
                        referenceGroupShape.UpdateText("textbox_source",
                            reference.Source.IsEmpty() ? "" : $"Source: {reference.Source.WrongSpeakToLeetSpeak(leetLevel)}");
                        referenceGroupShape.UpdateText("textbox_date",
                            (!reference.DatePublished.HasValue)
                                ? ""
                                : $"Date Published: {reference.DatePublished.Value:dd/MM/yyyy}");

                        var imagePath = Path.Combine(_mediaFolder, image);
                        if (_handleBlurryAreas)
                        {
                            imagePath = ModifyImage(reference, imagePath);
                        }

                        referenceGroupShape.ReplacePicture("picture_screenshot", imagePath);
                        if (referenceIndex > 0)
                        {
                            slideSequence.RemoveByShape(referenceGroupShape as IShape);
                            if (referenceSlide.Pictures.Count > 0)
                            {
                                referenceSlide.Pictures.RemoveAt(0);
                            }
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

            if (!_realAiNewsRender)
            {
                doc.Slides.Remove(thinkingSlideTemplate);
                doc.Slides.Remove(conclusionSlideTemplate);
            }

            #region conclusion slide

            if (!_realAiNewsRender)
            {
                conclusionSlide.UpdateText("conclusion_text", _puzzle.Conclusion.WrongSpeakToLeetSpeak(leetLevel));
                conclusionSlide.ReplacePicture("conclusion_puzzle", Path.Combine(_outputFolder, "Puzzle9.png"));
                doc.Slides.Add(thinkingSlide);
                doc.Slides.Add(conclusionSlide);
            }
            #endregion

            doc.Save(Path.Combine(_outputFolder, $"{_puzzle.FullTitle}.pptm"));
        }

        private void GenerateNarrationForSpeechelo()
        {
            var (timeline, narrations) = GenerateNarrationTimeline();
            
            const string startPhrase = "and, down the rabbit hole we go!";
            const double startPhraseDuration = 2;

            StringBuilder narrationWithPauses = new StringBuilder($"[startSpeech v=Loud startSpeech]{startPhrase}");

            //pause before first
            narrationWithPauses.Append(pause(narrations.First().Key.TotalSeconds - startPhraseDuration));

            for (var i = 0; i < narrations.Count(); i++)
            {
                //speak
                narrationWithPauses.Append($"{narrations[i].Value.Description}");

                //pause before next
                if (i != narrations.Count() - 1)
                {
                    var thisNarrationDuration = narrations[i].Value.ToNarrationDuration();

                    var waitSeconds = (narrations[i + 1].Key - narrations[i].Key).TotalSeconds - thisNarrationDuration;

                    ////Speechelo bug: adds an extra 3.5 seconds if the previous clip has a dot at the end.
                    //waitSeconds -= 3.5;

                    narrationWithPauses.Append(pause(waitSeconds));
                }

            }

            narrationWithPauses.Append("[endSpeech]");

            File.WriteAllTextAsync(Path.Combine(_outputFolder, "NarrationSpeechelo.txt"), narrationWithPauses.ToString());

            var narrationText = string.Join("\r",narrations.Select(_ => $"{_.Key.ToString(@"mm\:ss\.ff")} {_.Value.Description}").ToArray());

            File.WriteAllTextAsync(Path.Combine(_outputFolder, "Narration.txt"), $"{narrationText}");


        }

        private Func<double, string> pause = (double seconds) =>
        {
            StringBuilder pauseSb = new StringBuilder();
            if (seconds > 0)
            {
                var roundedSeconds = (int)seconds;
                for (var i = 0; i < roundedSeconds; i++)
                {
                    pauseSb.Append("[sPause sec=1 ePause]");
                }

                var remaining = Math.Round(seconds - roundedSeconds, 1);
                if (remaining > 0)
                {
                    pauseSb.Append($"[sPause sec={remaining} ePause]");
                }

            }
            return pauseSb.ToString();
        };

        private (string,List<KeyValuePair<TimeSpan,Reference>>) GenerateNarrationTimeline()
        {
            
            var narrations = new Dictionary<TimeSpan, Reference>();

            const double slideTransitionDuration = 0.88; //should be 0.7 but adjusting for unknown delay in powerpoint video rendering (accumulative)
            const double firstReferenceTransitionDuration = 2;
            const double referenceTransitionDuration = 0.4;
            const double referenceDurationModifier = -0.5;

            var currentTime = new TimeSpan();
            var timeline = new StringBuilder();

            foreach (var puzzlePiece in _puzzle.PuzzlePieces.OrderBy(_ => _.RenderOrder))
            {

                timeline.AppendLine($"{currentTime.ToString(@"mm\:ss")}\tPiece {puzzlePiece.RenderOrder} {puzzlePiece.Piece.Title}");

                currentTime=currentTime.Add( TimeSpan.FromSeconds(slideTransitionDuration + puzzlePiece.Piece.Duration));

                for(int referenceIndex=0;referenceIndex<puzzlePiece.Piece.References.Count;referenceIndex++)
                {
                    var reference = puzzlePiece.Piece.References[referenceIndex];
                    timeline.AppendLine($"\t{currentTime.ToString(@"mm\:ss\.ff")}\tReference {referenceIndex} \t{reference.Url.Replace("https://","").Limit(20)} \t{reference.Description}");

                    if (!reference.Description.IsEmpty())
                    {
                        narrations.Add( currentTime,reference);
                    }

                    var referenceTransDuration = referenceIndex == 0
                        ? firstReferenceTransitionDuration
                        : referenceTransitionDuration;

                    var modifier = referenceIndex == 0 ? 0 : referenceDurationModifier;

                    currentTime =currentTime.Add(TimeSpan.FromSeconds(referenceTransDuration + reference.Duration + modifier));

                    if (reference.Images.Count > 1)
                    {
                        for (var imageIndex = 1; imageIndex < reference.Images.Count; imageIndex++)
                        {
                            currentTime = currentTime.Add(TimeSpan.FromSeconds(referenceTransitionDuration + reference.Duration + modifier));
                        }
                    }
                }

            }

            return (timeline.ToString(),narrations.ToList());
        }
        
        private string ModifyImage(Reference reference, string image)
        {
            var imageEdit = reference.ImageEdits.FirstOrDefault(e => image.EndsWith(e.Image));
            if (imageEdit == null) return image;
            var modifiedImage = imageEdit.CreateNewImage(_mediaFolder);
            return modifiedImage;
        }

        private void FixSlidesDuration()
        {

            var puzzlePresentationPath = Path.Combine(_outputFolder, $"{_puzzle.FullTitle}.pptm");
            using var doc = Presentation.Open(puzzlePresentationPath);

            if (!_realAiNewsRender)
            {
                doc.Slides[0].SlideTransition.TimeDelay = _puzzle.Duration;
            }
            
            foreach (var puzzlePiece in _puzzle.PuzzlePieces)
            {
                var isFirstImageInPiece = true;
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

        public void Dispose()
        {
            string[] filePaths = Directory.GetFiles(Path.GetTempPath(), "blurred*");
            foreach (string filePath in filePaths)
            {
                File.Delete(filePath);
            }
        }
    }

    public class PuzzleTheme
    {
        public PieceBGColors PieceBgColor;
        public Color PieceTextColor;

        public enum PieceBGColors
        {
            Black,
            Blue
        }

    }
}