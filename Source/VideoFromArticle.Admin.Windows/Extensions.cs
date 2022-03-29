using FackCheckThisBitch.Common;
using NAudio.Wave;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using VideoFromArticle.Models;

namespace VideoFromArticle.Admin.Windows
{
    public static class Extensions
    {
        public static int IndexFromScreenPoint(this ListBox lst, Point point)
        {
            // Convert the location to the ListBox's coordinates.
            point = lst.PointToClient(point);

            // Return the index of the item at that position.
            return lst.IndexFromPoint(point);
        }

        public static TimeSpan GetMp3Duration(string filePath)
        {
            using (var mp3Reader = new Mp3FileReader(filePath))
            {
                TimeSpan duration = mp3Reader.TotalTime;
                return duration;
            }

        }

        public static string FilenameFromUrl(this string url)
        {
            var uri = new Uri(url);
            var segments = uri.Segments;
            var l = segments.Length;
            var fileName = segments[l - 1];
            return fileName;
        }

        public static string UrlWithoutQuerystring(this string url)
        {
            var uri = new Uri(url);
            var querystring = uri.Query;
            if (querystring.IsEmpty()) return url;
            return url.Replace(querystring, "");
        }

        public static string Folder(this Article article)
        {
            return Path.Combine(article.SlideshowFolder, article.Id);
        }

        public static string ArticleNarrationAudioFilePath(this Article article)
        {
            return Path.Combine(article.Folder(), $"{article.Title.Sanitize().Limit(30)}.mp3");
        }

        public static string ImageNarrationAudioFilePath(this Article article,ArticleImage image)
        {
            return Path.Combine(article.Folder(), image.Filename.Replace(".jpg",".mp3").Replace(".png",".mp3"));
        }

        public static double ProjectedDurationInSeconds(this Article article)
        {
            if (article.DurationInSeconds != 0)
            {
                return article.DurationInSeconds;
            }
            else
            {
                return article.Narration.Sanitize().Trim().Length / 16.14;
            }
        }

        public static bool NarrationFileExists(this Article article)
        {
            var audioFile = article.ArticleNarrationAudioFilePath();
            return File.Exists(audioFile);
        }

        public static TimeSpan ReadNarrationDuration(this Article article)
        {
            var audioFile = article.ArticleNarrationAudioFilePath();
            if (!File.Exists(audioFile)) return new TimeSpan();
            using (var mp3Reader = new Mp3FileReader(audioFile))
            {
                var audioDuration = mp3Reader.TotalTime;
                return audioDuration;
            }
        }

        public static TimeSpan ReadNarrationDuration(this Article article, ArticleImage image)
        {
            var audioFile = article.ImageNarrationAudioFilePath(image);
            if (!File.Exists(audioFile)) return new TimeSpan();
            using (var mp3Reader = new Mp3FileReader(audioFile))
            {
                var audioDuration = mp3Reader.TotalTime;
                return audioDuration;
            }
        }

        public static string Diagnostics(this Article article)
        {
            StringBuilder result = new StringBuilder();
            bool problem = false;

            if (!article.Images.Any())
            {
                problem = true;
                result.Append("No Images found. ");
            }
            else
            {
                result.Append($"Images: {article.Images.Count}. ");
                int filesNotFound = 0;
                foreach (var imageFileName in article.Images.Select(_ => _.Filename))
                {

                    if (!File.Exists(Path.Combine(article.SlideshowFolder, article.Id, imageFileName)))
                    {
                        filesNotFound++;
                    }
                }
                if (filesNotFound > 0)
                {
                    problem = true;
                    result.Append($"{filesNotFound} image files missing. ");
                }
            }

            if (!article.NarrationPerImage.Value)
            {
                var audioFile = article.ArticleNarrationAudioFilePath();
                if (!File.Exists(audioFile))
                {
                    problem = true;
                    result.Append("No Audio found. ");
                }
                else
                {
                    var audioDuration = article.ReadNarrationDuration();
                    result.Append($"Single Audio: {audioDuration:mm\\:ss\\:FF} {Math.Ceiling(audioDuration.TotalSeconds)} sec. ");

                    if (article.Images.Any())
                    {
                        var durationPerArticleImage = audioDuration.TotalSeconds / article.Images.Count;
                        result.Append($"{Math.Round(durationPerArticleImage, 2)} sec per image. ");
                    }
                }

                if (!article.Narration.IsEmpty())
                {
                    var charCount = article.Narration.Trim().Length;
                    if (article.Images.Any())
                    {
                        result.Append($"{Math.Round((decimal)charCount / article.Images.Count, 1)} letters per image. ");
                    }

                }
            }
            else
            {
                var articleDirectory = new DirectoryInfo(article.Folder());
                if (articleDirectory.Exists)
                {
                    var audioFiles = articleDirectory.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
                    if (audioFiles.Length == 0)
                    {
                        problem = true;
                        result.Append("Audio files missing. ");
                    }
                    else
                    {
                        result.Append($"{audioFiles.Length}/{article.Images.Count} Audio files. Duration : {Math.Ceiling(article.DurationInSeconds)} sec. ");
                    }
                }
                else
                {

                }
            }



            return $"{(problem ? "!!! " : "")}{result}";
        }

        public static void EnsureFolder(this Article article)
        {
            if (!Directory.Exists(article.Folder()))
            {
                Directory.CreateDirectory(article.Folder());
            }
        }

        public static string Folder(this Slideshow slideshow)
        {
            var result = slideshow == null
                ? null
                : Path.Combine(Configuration.Instance().DataFolder, slideshow.SanitizedTitle);
            return result;
        }

        public static string FilePath(this Slideshow slideshow)
        {
            var result = slideshow == null
                ? null
                : Path.Combine(slideshow.Folder(), "slideshow.json");
            return result;
        }


    }
}
