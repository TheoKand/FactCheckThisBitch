using SixLabors.ImageSharp;
using System;
using System.IO;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Processing;
using FactCheckThisBitch.Models;
using SixLabors.ImageSharp.Drawing.Processing;

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
            File.Copy(emptyPuzzleImagePath, Path.Combine(_outputFolder,"Puzzle0.png"));

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
//                        ctx.Mutate(x => x.DrawImage(logo, new Point(1,1), opacity: 0.5f));

                        using (var pieceWithKeywordsImage = Image.Load(pieceImageWithKeywordsPath))
                        {
                            var location = _puzzle.GetPieceCoordinates(puzzlePiece.Index);


                            ctx.DrawImage(pieceWithKeywordsImage, new Point(location.X,location.Y),1);
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
                    var pieceCoordinates = Extensions.GetPieceCoordinates(_puzzle,  puzzlePiece.Index);

                    Font font = SystemFonts.CreateFont("Arial", 10); // for scaling water mark size is largely ignored.
                    using (var pieceImageWithKeywords = pieceImage.Clone(ctx =>
                    {
                        //ctx.ApplyScalingWaterMark(font, puzzlePiece.Piece.GetKeywordsText(),pieceCoordinates,  Color.White, 5, false)

                        ImageSharpExtensions.ApplyScalingWaterMark(ctx, font, puzzlePiece.Piece.GetKeywordsText(),
                            pieceCoordinates.KeywordsBox, Color.White, 5, false);
                    }
                        ))
                    {
                        pieceImageWithKeywords.Save(pieceImgeWithKeywordsPath);
                    }

                    //using (var img2 = img.Clone(ctx => ctx.ApplyScalingWaterMark(font, LongText, Color.HotPink, 5, true)))
                    //{
                    //    img2.Save("output/wrapped.png");
                    //}
                }
            }
        }

        public void Dispose()
        {
            
        }
    }
}