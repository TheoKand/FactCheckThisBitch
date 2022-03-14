using FackCheckThisBitch.Common;
using NAudio.Wave;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

        public static string Folder(this SlideshowArticle input)
        {
            return Path.Combine(Configuration.Instance().DataFolder, input.Id);
        }

        public static string ImagesFolder(this SlideshowArticle input)
        {
            return Path.Combine(Configuration.Instance().DataFolder, input.Id, "images");
        }

        public static string NarrationAudioFilePath(this SlideshowArticle input)
        {
            return Path.Combine(input.Folder(), $"{input.Title.Sanitize().Limit(30)}.mp3");
        }


        public static void SanitizeNarration(this SlideshowArticle input)
        {
            var narration = input.Narration.Replace("(pictured)", " ");

            input.Narration = narration;
        }

        public static bool NarrationFileExists(this SlideshowArticle input)
        {
            var audioFile = input.NarrationAudioFilePath();
            return File.Exists(audioFile);
        }

        public static TimeSpan ArticleNarrationDuration(this SlideshowArticle input)
        {
            var audioFile = input.NarrationAudioFilePath();
            using (var mp3Reader = new Mp3FileReader(audioFile))
            {
                var audioDuration = mp3Reader.TotalTime;
                return audioDuration;
            }
        }

        public static string Diagnostics(this SlideshowArticle input)
        {
            StringBuilder result = new StringBuilder();
            bool problem = false;

            if (!input.Images.Any())
            {
                problem = true;
                result.Append("Images missing. ");
            }
            else
            {
                result.Append($"Images: {input.Images.Count}. ");
                foreach (var imageFileName in input.Images.Select(_ => _.Filename))
                {
                    int filesNotFound = 0;
                    if (!File.Exists(Path.Combine(input.ImagesFolder(), imageFileName)))
                    {
                        filesNotFound++;
                    }

                    if (filesNotFound > 0)
                    {
                        problem = true;
                        result.Append($"{filesNotFound} image files missing.");
                    }
                }
            }

            var audioFile = input.NarrationAudioFilePath();
            if (!File.Exists(audioFile))
            {
                problem = true;
                result.Append("Audio missing. ");
            }
            else
            {
                using(var mp3Reader = new Mp3FileReader(audioFile))
                {
                    var audioDuration =input.ArticleNarrationDuration();
                    result.Append($"Audio: {Math.Ceiling(audioDuration.TotalSeconds)} sec. ");

                    var durationPerArticleImage = audioDuration.TotalSeconds / input.Images.Count;
                    result.Append($"{Math.Round(durationPerArticleImage, 1)} sec per slide");
                }
            }

            return $"{(problem ? "!!! " : "")}{result}";
        }

    }
}
