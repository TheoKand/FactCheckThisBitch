using FackCheckThisBitch.Common;
using Newtonsoft.Json;
using Syncfusion.Presentation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VideoFromArticle.Models;
using Color = System.Drawing.Color;

namespace FactCheckThisBitch.Render
{
    public class VideoFromArticleRenderer : IDisposable
    {
        const int MinimumSecondsPerSlide = 7;

        private readonly Slideshow _slideshow;
        private readonly string _template;
        private readonly int _introDuration;
        private readonly string _dataFolder;
        private readonly string _videoFromArticleTemplatesFolder;

        public VideoFromArticleRenderer(Slideshow slideshow, string template, int introDuration, string dataFolder,
            string videoFromArticleTemplatesFolder)
        {
            _slideshow = slideshow;
            _template = template;
            _dataFolder = dataFolder;
            _introDuration = introDuration;
            _videoFromArticleTemplatesFolder = videoFromArticleTemplatesFolder;
        }

        private string VideoOutputFolder =>
            Path.Combine(_videoFromArticleTemplatesFolder, "_output", _slideshow.SanitizedTitle);

        private string PowerpointTemplate =>
            Path.Combine(_videoFromArticleTemplatesFolder, _template, $"{_template}Slideshow.pptm");

        private string PowerpointOutput => Path.Combine(VideoOutputFolder, $"{_slideshow.SanitizedTitle}.pptm");

        private string OpenShotFileTemplate =>
            Path.Combine(_videoFromArticleTemplatesFolder, _template, $"{_template}Template.osp");

        private string OpenShotFileOutput => Path.Combine(VideoOutputFolder, $"{_slideshow.SanitizedTitle}.osp");

        public void Render()
        {
            PrepareOutputFolder();
            CreatePresentationFromTemplate();
            FixSlidesDuration();
            CopyAudioFiles();
            GenerateTextFiles();
            CreateOpenShotVideoFile();
            CleanUp();
            OpenOutputFolder();
        }

        private void PrepareOutputFolder()
        {
            //duplicate video template directory for the new video being exported
            var templateBaseFolder = Path.Combine(_videoFromArticleTemplatesFolder, _template);
            var templateBaseFolderDirectory = new DirectoryInfo(templateBaseFolder);

            if (Directory.Exists(VideoOutputFolder))
            {
                Directory.Delete(VideoOutputFolder, true);
            }

            templateBaseFolderDirectory.CopyTo(VideoOutputFolder);

            #region cleanup template folder

            //region delete all narration files
            string narrationFolder = Path.Combine(VideoOutputFolder, "narration");
            Directory.Delete(narrationFolder, true);
            Directory.CreateDirectory(narrationFolder);

            #endregion
        }

        private void CreatePresentationFromTemplate()
        {
            const int maximumSecondsPerSlideBeforeRecycle = 10;

            using IPresentation doc = Presentation.Open(PowerpointTemplate);

            ISlide articleImageTemplateSlide = doc.Slides[0];

            var titleColors = new[] { Color.White, Color.Red, Color.Yellow, Color.LawnGreen };

            #region create one slide for each article image

            var articleIndex = 0;
            foreach (var article in _slideshow.Articles.OrderBy(_ => _.Order))
            {
                article.RecycledImages.Clear();
                article.RecycledImages =
                    JsonConvert.DeserializeObject<List<ArticleImage>>(JsonConvert.SerializeObject(article.Images));

                if (article.RecycleImages.Value)
                {
                    //if duration per slide is too high, recycle the same slides
                    var durationPerArticleImage = article.DurationInSeconds / article.Images.Count;
                    if (durationPerArticleImage < MinimumSecondsPerSlide)
                    {
                        throw new Exception(
                            $"Duration per image for article {article.Title} is {durationPerArticleImage}, minimum is {MinimumSecondsPerSlide} ");
                    }

                    if (durationPerArticleImage > maximumSecondsPerSlideBeforeRecycle)
                    {
                        var startToRecycleAtThisPointInTime =
                            article.Images.Count * maximumSecondsPerSlideBeforeRecycle;
                        var pointInTime = startToRecycleAtThisPointInTime;
                        var recycleImageIndex = 0;
                        while (pointInTime < article.DurationInSeconds - (maximumSecondsPerSlideBeforeRecycle + 3))
                        {
                            var imageToRecycle = article.Images[recycleImageIndex];
                            article.RecycledImages.Add(imageToRecycle);
                            recycleImageIndex++;
                            if (recycleImageIndex > article.Images.Count - 1)
                            {
                                recycleImageIndex = 0;
                            }

                            pointInTime += maximumSecondsPerSlideBeforeRecycle;
                        }
                    }
                }

                var secondsPerArticleImage = article.DurationInSeconds / article.RecycledImages.Count;

                for (var imageIndex = 0; imageIndex < article.RecycledImages.Count; imageIndex++)
                {
                    var articleImage = article.RecycledImages[imageIndex];
                    var imageSlide = articleImageTemplateSlide.Clone();
                    var imageSlideDuration = article.NarrationPerImage.Value
                        ? articleImage.DurationInSeconds
                        : secondsPerArticleImage;

                    imageSlide.Name = $"{article.Id}-image{imageIndex}";

                    var slideTitle = article.Title;
                    //if this image has a caption, show that instead of article title
                    if (articleImage.Caption != article.Title &&
                        articleImage.Caption.IsNotEmpty())
                    {
                        slideTitle = articleImage.Caption;
                    }

                    var txtLabel = imageSlide.UpdateText("txtTitle", slideTitle);
                    var txtSource = imageSlide.UpdateText("txtSource", article.Source);
                    var txtUrl =
                        imageSlide.UpdateText("txtUrl",
                            article.Url); //.Replace("https://", "").Replace("http://", "").Replace("www.", ""));
                    var txtDate = imageSlide.UpdateText("txtDate", article.Published?.ToString("dd/MM/yyyy"));
                    txtLabel.SetShapeTextColor(titleColors[articleIndex % titleColors.Length]);

                    if (txtSource != null && txtSource.TextBody.Text.Length > 10)
                    {
                        txtSource.TextBody.WrapText = false;
                        txtSource.TextBody.FitTextOption = FitTextOption.ShrinkTextOnOverFlow;
                    }

                    //hide controls not used
                    imageSlide.RemoveShapeIfTextEmpty("txtSource");
                    imageSlide.RemoveShapeIfTextEmpty("txtUrl");
                    imageSlide.RemoveShapeIfTextEmpty("txtDate");

                    //hide title for last slide of article (unless it's an image caption)
                    if (imageIndex == article.RecycledImages.Count - 1 && slideTitle == article.Title)
                    {
                        imageSlide.HideShapeAndRemoveAnimations(txtLabel);
                    }
                    else if (slideTitle != article.Title && imageIndex == 0)
                    {
                        //the title should appear quicker for slides with a specific caption title
                        var titleLabelAnimations = imageSlide.Timeline.MainSequence.GetEffectsByShape(txtLabel);
                        titleLabelAnimations[0].Timing.TriggerDelayTime = 1f;
                    }


                    //load and center picture
                    var photoFile = Path.Combine(_dataFolder, article.Id, articleImage.Filename);
                    var photoSizeOnDisk = photoFile.GetImageSize();
                    //get best fit
                    var (newWidth, newHeight) = ImageSharpExtensions.BestFit(photoSizeOnDisk.Width,
                        photoSizeOnDisk.Height, (int)imageSlide.SlideSize.Width, (int)imageSlide.SlideSize.Height);

                    imageSlide.ReplacePicture("image1", photoFile);
                    var imageShape = imageSlide.ShapeByName("image1") as IShape;

                    bool largerPictures = false;

                    var sizeReduceFactor = largerPictures ? 1.1f : 0.9f;
                    imageSlide.CenterShape(imageShape, (int)Math.Round(newWidth * sizeReduceFactor, 0),
                        (int)Math.Round(newHeight * sizeReduceFactor, 0));

                    imageShape.Top += largerPictures ? imageShape.Height * 0.1 : 0;

                    //progressBar
                    var progressBar = imageSlide.ShapeByName("progressBar");
                    progressBar.Width = txtUrl.Width * imageIndex / (article.RecycledImages.Count - 1);

                    if (imageIndex == 0)
                    {
                        imageSlide.HideShapeAndRemoveAnimations("progressBarEmpty");
                    }
                    if (imageIndex > 0)
                    {
                        //remove animations of textboxes except for the first slide of an article
                        imageSlide.HideShapeAndRemoveAnimations(txtUrl);

                        imageSlide.RemoveAnimationsForShape("txtTitle");
                        imageSlide.RemoveAnimationsForShape("txtSource");
                        imageSlide.RemoveAnimationsForShape("txtDate");
                        imageSlide.RemoveAnimationsForShape("logo");
                        imageSlide.RemoveAnimationsForShape("progressBar");
                        imageSlide.RemoveAnimationsForShape("progressBarEmpty");
                    }

                    imageSlide.SlideTransition.TimeDelay = (float)imageSlideDuration;

                    ISequence sequence = imageSlide.Timeline.MainSequence;
                    var imageAnimations =
                        sequence.GetEffectsByShape(imageSlide.Shapes.First(_ => _.ShapeName == "image1") as IShape);
                    imageAnimations[1].Behaviors[0].Timing.Duration = (float)imageSlideDuration - 1.5f;
                    imageAnimations[2].Behaviors[0].Timing.Duration = (float)imageSlideDuration - 1.5f;

                    //show next article teaser
                    var groupNext = imageSlide.ShapeByName("GroupNext") as IGroupShape;
                    if (article.NextPreview.Value && articleIndex < _slideshow.Articles.Count - 1 &&
                        imageIndex == article.RecycledImages.Count / 2)
                    {
                        var nextArticle = _slideshow.Articles[articleIndex + 1];
                        var howManyImagesLeftInArticle = article.RecycledImages.Count - imageIndex - 1;
                        //TODO: calculate time left in article,  if narration per image

                        if (article.NarrationPerImage.Value)
                        {
                             imageSlide.HideShapeAndRemoveAnimations("txtNextWhen");
                        }
                        else
                        {
                            var howManySecondsLeftInArticle =
                                (int)Math.Round(secondsPerArticleImage * howManyImagesLeftInArticle, 0);
                            groupNext.UpdateText("txtNextWhen", $"in {howManySecondsLeftInArticle} sec");
                        }

                        groupNext.UpdateText("txtNextTitle", $"{nextArticle.Title.Limit(103, "...")}");
                        
                        var nextArticleFirstPicture =
                            Path.Combine(_dataFolder, nextArticle.Id, nextArticle.Images[0].Filename);
                        groupNext.ReplacePicture("NextPicture", nextArticleFirstPicture);
                    }
                    else
                    {
                        groupNext.Left = imageSlide.SlideSize.Width + 100;
                        imageSlide.RemoveAnimationsForShape("GroupNext");
                    }

                    doc.Slides.Add(imageSlide);
                }

                articleIndex++;
            }

            #endregion

            doc.Slides.Remove(articleImageTemplateSlide);

            doc.Save(PowerpointOutput);
        }

        private void FixSlidesDuration()
        {
            using var doc = Presentation.Open(PowerpointOutput);

            var slideIndex = 0;

            foreach (var article in _slideshow.Articles.OrderBy(_ => _.Order))
            {
                var totalArticleSeconds = article.DurationInSeconds;
                var secondsPerArticleImage = totalArticleSeconds / article.RecycledImages.Count;

                for (var imageIndex = 0; imageIndex < article.RecycledImages.Count; imageIndex++)
                {
                    var imageSlide = doc.Slides[slideIndex];
                    var articleImage = article.RecycledImages[imageIndex];

                    var imageSlideDuration = article.NarrationPerImage.Value
                        ? articleImage.DurationInSeconds
                        : secondsPerArticleImage;

                    imageSlide.SlideTransition.TimeDelay = (float)imageSlideDuration;

                    slideIndex++;
                }
            }

            doc.Save(PowerpointOutput);
        }

        private void CopyAudioFiles()
        {
            for (var articleIndex = 0; articleIndex < _slideshow.Articles.Count; articleIndex++)
            {
                var article = _slideshow.Articles[articleIndex];

                if (article.NarrationPerImage.Value)
                {

                    for (var imageIndex = 0; imageIndex < article.Images.Count; imageIndex++)
                    {
                        var articleImage = article.Images[imageIndex];
                        var sourceFile = Path.Combine(_dataFolder, article.Id,
                            articleImage.Filename.Replace(".jpg", ".mp3").Replace(".png", ".mp3"));
                        var destFile = Path.Combine(VideoOutputFolder, "narration", $"Narration{articleIndex+1:00}.{imageIndex+1:00}.mp3");
                        try
                        {
                            File.Copy(sourceFile, destFile, true);
                        }
                        catch
                        {
                        }
                    }


                }
                else
                {
                    var sourceFile = Path.Combine(_dataFolder, article.Id, $"{article.Title.Sanitize().Limit(30)}.mp3");
                    var destFile = Path.Combine(VideoOutputFolder, "narration", $"Narration{articleIndex+1:00}.mp3");
                    try
                    {
                        File.Copy(sourceFile, destFile, true);
                    }
                    catch
                    {
                    }
                }

            }
        }

        private void GenerateTextFiles()
        {
            StringBuilder metadata = new StringBuilder();
            StringBuilder references = new StringBuilder();

            if (_introDuration > 0)
            {
                metadata.AppendLine("00:00 Intro");
            }

            var currentTimeInVideo = TimeSpan.FromSeconds(_introDuration);

            foreach (var article in _slideshow.Articles.OrderBy(_ => _.Order))
            {
                var chapter = currentTimeInVideo.ToString("mm\\:ss");

                metadata.AppendLine($"{chapter} {article.Title}");
                references.AppendLine($"👉 {article.Title} {article.Url}");

                currentTimeInVideo = currentTimeInVideo.Add(TimeSpan.FromSeconds(article.DurationInSeconds));
            }

            metadata.AppendLine($"\nReferences:\n{references}");
            File.WriteAllTextAsync(Path.Combine(VideoOutputFolder, "Metadata.txt"), $"{metadata}");
        }

        private void CreateOpenShotVideoFile()
        {
            var openShotFileTemplate = File.ReadAllText(OpenShotFileTemplate);
            string resultOspFileContents = openShotFileTemplate;

            //set export/import paths to new video output folder
            resultOspFileContents = new Regex("\"export_path\": \"([^\"]*)").Replace(resultOspFileContents,
                $"\"export_path\": \"{VideoOutputFolder.Replace("\\", "/")}");
            resultOspFileContents = new Regex("\"import_path\": \"([^\"]*)").Replace(resultOspFileContents,
                $"\"import_path\": \"{VideoOutputFolder.Replace("\\", "/")}");

            //replace common assets folder (because it's relative and the video folder is not on the same level)
            resultOspFileContents = resultOspFileContents.Replace("../_commonAssets/", "../../_commonAssets/");

            //replace slideshow video file. MUST SAVE POWERPOINT AS VIDEO WITH [SAMENAME].MP4
            resultOspFileContents = resultOspFileContents.Replace($"{_template}Slideshow.mp4",
                $"{_slideshow.SanitizedTitle}Slideshow.mp4");

            File.WriteAllTextAsync(OpenShotFileOutput, $"{resultOspFileContents}");
        }

        private void CleanUp()
        {
            //delete unneeded files from output folder
            File.Delete(Path.Combine(VideoOutputFolder, $"{_template}Slideshow.pptm"));
            File.Delete(Path.Combine(VideoOutputFolder, $"{_template}Template.osp"));
        }

        private void OpenOutputFolder()
        {
            new Process { StartInfo = new ProcessStartInfo(VideoOutputFolder) { UseShellExecute = true } }.Start();
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