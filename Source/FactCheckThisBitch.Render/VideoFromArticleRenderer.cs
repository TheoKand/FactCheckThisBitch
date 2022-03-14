using FackCheckThisBitch.Common;
using FactCheckThisBitch.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Syncfusion.Presentation;
using Syncfusion.Presentation.SlideTransition;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using VideoFromArticle.Models;
using Color = System.Drawing.Color;

namespace FactCheckThisBitch.Render
{
    public class VideoFromArticleRenderer : IDisposable
    {
        private readonly Slideshow _slideshow;
        private readonly string _template;
        private readonly string _assetsFolder;
        private readonly string _outputFolder;
        private readonly string _dataFolder;

        public VideoFromArticleRenderer(Slideshow slideshow, string template, string assetsFolder, string outputFolder, string dataFolder)
        {
            _slideshow = slideshow;
            _template = template;
            _assetsFolder = assetsFolder;
            _outputFolder = outputFolder;
            _dataFolder = dataFolder;
        }

        public void Render()
        {
            PrepareOutputFolder();
            CreatePresentationFromTemplate();
            FixSlidesDuration();
            CopyAudioFiles();
            OpenOutputFolder();
            //TODO: copy audio files to output
            //create video project from template
        }

        private void OpenOutputFolder()
        {
            new Process { StartInfo = new ProcessStartInfo(_outputFolder) { UseShellExecute = true } }.Start();
        }

        private void CopyAudioFiles()
        {
            foreach (var article in _slideshow.Articles.OrderBy(_ => _.Order))
            {
                var sourceFile = Path.Combine(_dataFolder, article.Id, $"{article.Title.Sanitize().Limit(30)}.mp3");
                var destFile = Path.Combine(_outputFolder, $"{article.Title.Sanitize().Limit(30)}.mp3");
                try
                {
                    File.Copy(sourceFile, destFile, true);
                }
                catch
                {
                }

            }

        }

        private void PrepareOutputFolder()
        {
            if (!Directory.Exists(_outputFolder))
            {
                Directory.CreateDirectory(_outputFolder);
            }

            
        }

        private void CreatePresentationFromTemplate()
        {
            const int MaximumSecondsPerArticleImage = 10;

            var templatePath = Path.Combine(_assetsFolder, "Powerpoint", _template);
            using IPresentation doc = Presentation.Open(templatePath);

            ISlide articleImageTemplateSlide = doc.Slides[0];

            var titleColors = new[] { Color.White, Color.Red, Color.Yellow, Color.LawnGreen };

            #region create one slide for each article image

            var articleIndex = 0;
            foreach (var article in _slideshow.Articles.OrderBy(_ => _.Order))
            {
                article.RecycledImages.Clear();
                article.RecycledImages = JsonConvert.DeserializeObject<List<ArticleImage>>(JsonConvert.SerializeObject(article.Images));

                //if duration per slide is too high, recycle the same slides
                var durationPerArticleImage = article.DurationInSeconds / article.Images.Count;
                if (durationPerArticleImage > MaximumSecondsPerArticleImage)
                {
                    var startToRecycleAtThisPointInTime = article.Images.Count * MaximumSecondsPerArticleImage;
                    var pointInTime = startToRecycleAtThisPointInTime;
                    var recycleImageIndex = 0;
                    while (pointInTime < article.DurationInSeconds - (MaximumSecondsPerArticleImage+3))
                    {
                        var imageToRecycle = article.Images[recycleImageIndex];
                        article.RecycledImages.Add(imageToRecycle);
                        recycleImageIndex++;
                        if (recycleImageIndex > article.Images.Count - 1)
                        {
                            recycleImageIndex = 0;
                        }

                        pointInTime += MaximumSecondsPerArticleImage;
                    }
                }
                var totalArticleSeconds = article.DurationInSeconds;
                var secondsPerArticleImage = totalArticleSeconds / article.RecycledImages.Count;

                for (var imageIndex=0; imageIndex< article.RecycledImages.Count;imageIndex++)
                {
                    var imageSlide = articleImageTemplateSlide.Clone();
                    imageSlide.Name = $"{article.Id}-image{imageIndex}";
                    var titleLabel = imageSlide.UpdateText("txtTitle", article.Title);
                    var sourceLabel = imageSlide.UpdateText("txtSource", article.Source);
                    imageSlide.UpdateText("txtUrl", article.Url);
                    var dateLabel = imageSlide.UpdateText("txtDate", article.Published?.ToString("dd/MM/yyyy"));

                    titleLabel.SetShapeTextColor(titleColors[articleIndex % titleColors.Length]);

                    if (sourceLabel != null && sourceLabel.TextBody.Text.Length > 10)
                    {
                        sourceLabel.TextBody.WrapText = false;
                        sourceLabel.TextBody.FitTextOption = FitTextOption.ShrinkTextOnOverFlow;
                    }

                    //load and center picture
                    var photoFile = Path.Combine(_dataFolder, article.Id, "images",
                        article.RecycledImages[imageIndex].Filename);
                    var photoSizeOnDisk = photoFile.GetImageSize();
                    //get best fit
                    var (newWidth, newHeight) = ImageSharpExtensions.BestFit(photoSizeOnDisk.Width,
                        photoSizeOnDisk.Height, (int)imageSlide.SlideSize.Width, (int)imageSlide.SlideSize.Height);

                    imageSlide.ReplacePicture("image1", photoFile);
                    var imageShape = imageSlide.ShapeByName("image1") as IShape;
                    var sizeReduceFactor = 0.9f;
                    imageSlide.CenterShape(imageShape, (int)Math.Round(newWidth * sizeReduceFactor, 0), (int)Math.Round(newHeight * sizeReduceFactor, 0));

                    //remove animations of textboxes except for the first slide of an article
                    if (imageIndex > 0)
                    {
                        imageSlide.RemoveAnimationsForShape("txtTitle");
                        imageSlide.RemoveAnimationsForShape("txtSource");
                        imageSlide.RemoveAnimationsForShape("txtUrl");
                        imageSlide.RemoveAnimationsForShape("txtDate");
                        imageSlide.RemoveAnimationsForShape("logo");
                    }

                    imageSlide.SlideTransition.TimeDelay = (float)secondsPerArticleImage;

                    ISequence sequence = imageSlide.Timeline.MainSequence;
                    var imageAnimations = sequence.GetEffectsByShape(imageSlide.Shapes.First(_=>_.ShapeName== "image1") as IShape);
                    imageAnimations[1].Behaviors[0].Timing.Duration = (float)secondsPerArticleImage-1.5f;
                    imageAnimations[2].Behaviors[0].Timing.Duration = (float)secondsPerArticleImage-1.5f;

                    //imageSlide.RemoveAnimationsForShape("image1");
                    //imageSlide.RemoveAnimationsForShape("txtTitle");
                    //imageSlide.RemoveAnimationsForShape("txtUrl");
                    //imageSlide.RemoveAnimationsForShape("txtDate");
                    //imageSlide.RemoveAnimationsForShape("txtSource");

                    doc.Slides.Add(imageSlide);
                }

                articleIndex++;
            }

            #endregion

            doc.Slides.Remove(articleImageTemplateSlide);

            doc.Save(Path.Combine(_outputFolder, $"{_slideshow.FullTitle}.pptm"));
        }

        private void FixSlidesDuration()
        {

            var presentationPath = Path.Combine(_outputFolder, $"{_slideshow.FullTitle}.pptm");
            using var doc = Presentation.Open(presentationPath);

            var slideIndex = 0;

            foreach (var article in _slideshow.Articles.OrderBy(_ => _.Order))
            {
                var totalArticleSeconds = article.DurationInSeconds;
                var secondsPerArticleImage = totalArticleSeconds / article.RecycledImages.Count;

                for (var imageIndex = 0; imageIndex < article.RecycledImages.Count; imageIndex++)
                {
                    var imageSlide = doc.Slides[slideIndex];
                    imageSlide.SlideTransition.TimeDelay = (float) secondsPerArticleImage;

                    slideIndex++;
                }
            }

            doc.Save(presentationPath);
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

}