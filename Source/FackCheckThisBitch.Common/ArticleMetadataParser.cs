using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FackCheckThisBitch.Common
{
    public class ArticleMetadataParser
    {
        private static IDictionary<string, IEnumerable<string>> MetadataProperties =
            new Dictionary<string, IEnumerable<string>>()
            {
                { "title", new[] { "title" } },
                { "description", new[] { "description" } },
                { "author", new[] { "author" } },
                { "site_name", new[] { "site_name" } },
                { "original-source", new[] { "original-source" } },
                { "datePublished", new[] { "datePublished", "published", "published_time" } },
                { "keywords", new[] { "keywords" } },
                { "image", new[] { "image" } }
            };

        private readonly HttpClient _client;
        private readonly string _url;

        public ArticleMetadataParser(string url)
        {
            _client = new HttpClient();
            _url = url;
        }

        public async Task<IDictionary<string, string>> Download()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var result = new Dictionary<string, string>();

            _client.DefaultRequestHeaders.Clear();
            using (HttpResponseMessage response = await _client.GetAsync(new Uri(_url)))
            {
                response.EnsureSuccessStatusCode();

                using (HttpContent content = response.Content)
                {
                    #region get html
                    string html;
                    using (var sr = new StreamReader(await content.ReadAsStreamAsync(), Encoding.GetEncoding("utf-8")))
                    {
                        html = (await sr.ReadToEndAsync()).StripLinefeeds();
                    }
                    #endregion

                    #region parse metadata
                    foreach (var propertyName in MetadataProperties.Keys)
                    {
                        foreach (var possibleMeta in MetadataProperties[propertyName])
                        {
                            var propertyValue = TryGetMetadataProperty(html, possibleMeta);
                            if (!propertyValue.IsEmpty())
                            {
                                if (!result.ContainsKey(propertyName))
                                {
                                    result.Add(propertyName, propertyValue);
                                }
                            }
                        }
                    }
                    #endregion

                    #region parse images

                    var images = GetAdditionalImages(html);
                    result.Add("images", images != null ? string.Join("\n", images.Select( _=>_.image)) : null);

                    var imagesWithCaptions = images.Select(_ => $"{_.image}\t{_.caption}").ToList();
                    result.Add("imagesWithCaptions", images != null ? string.Join("\n", imagesWithCaptions) : null);

                    #endregion
                }
            }

            return result;
        }


        private List<(string image,string caption)> GetAdditionalImages(string content)
        {
            //TODO: improve to catch more images from various news sites

            //TODO: catch this twitter poster pattern:
            //  <video preload="none" playsinline="" disablepictureinpicture="" 
            //  poster = "https://pbs.twimg.com/ext_tw_video_thumb/1503288402346749953/pu/img/yk_rRp-uj9G41QMY.jpg"
            //  src = "blob:https://platform.twitter.com/2fe86960-8a97-4ca1-96bc-7b6776919871"

            string[] contentPatterns =
            {
                "<\\s*div[^>]*itemprop\\s*=\\s*[\\\"']articleBody[\\\"'][^>]*>(.*)<\\/\\s*div>",
                @"<\s*article[^>]*>(.*)<\/article>"
            };

            var article = GetFirstRegExMatch(content, contentPatterns);
            if (article == null) return null;

            string[] imagePatterns =
            {
                "<\\s*img[^>]* data-src\\s*=\\s*[\"']([^'\"]*)[\"'][^>]*>",
                "<\\s*img[^>]* src\\s*=\\s*[\"']([^'\"]*)[\"'][^>]*>"
            };
            var images = GetAllMatches(article, imagePatterns, " alt=", new[] { "jpg", "png", "tiff" });
            return images;
        }

        private string TryGetMetadataProperty(string content, string property)
        {
            string patternTemplate = "<meta[^\"]*\"propertyPlaceholder\"\\s*content=\"([^\"]*)";
            var patterns = new string[]
            {
                patternTemplate.Replace("propertyPlaceholder", $"og:{property}"),
                patternTemplate.Replace("propertyPlaceholder", $"{property}"),
                patternTemplate.Replace("propertyPlaceholder", $"article:{property}"),
                patternTemplate.Replace("propertyPlaceholder", $"article.{property}"),
                $"\"{property}\":\"([^\"]*)\""
            };
            var result = GetFirstRegExMatch(content, patterns);
            if (result != null) return result;

            patternTemplate = "itemprop[^\"]*\"propertyPlaceholder\"\\s*content=\\\"([^\\\"]*)";
            patterns = new string[]
            {
                patternTemplate.Replace("propertyPlaceholder", $"og:{property}"),
                patternTemplate.Replace("propertyPlaceholder", $"{property}"),
                patternTemplate.Replace("propertyPlaceholder", $"article:{property}"),
                patternTemplate.Replace("propertyPlaceholder", $"article.{property}"),
                $"\"{property}\":\"([^\"]*)\""
            };
            result = GetFirstRegExMatch(content, patterns);
            if (result != null) return result;

            return result;
        }

        private string GetFirstRegExMatch(string content, string[] patterns)
        {
            foreach (string pattern in patterns)
            {
                var result = Regex.Match(content, pattern);
                if (result.Groups.Count > 1)
                {
                    return result.Groups[1].Value.HtmlDecode();
                }
            }

            return null;
        }

        private string AltFromImageTag(string imageTag)
        {
            var match = Regex.Match(imageTag, "alt\\s*=\\s*\"([^\"]*)\"");
            if (!match.Success) return "";
            var alt = match.Groups[1].Value.HtmlDecode();
            return alt;
        }

        private List<(string,string)> GetAllMatches(string content, string[] patterns, string imageElementMustContain = null, string[] imageUrlMustContainOneOfThese = null)
        {
            var result = new List<(string, string)>();
            foreach (var pattern in patterns)
            {
                var matches = Regex.Matches(content, pattern);
                foreach (Match match in matches)
                {
                    if (match.Groups.Count > 1)
                    {
                        var imageElement = match.Groups[0].Value;
                        var imageUrl = match.Groups[1].Value.HtmlDecode();
                        var imageCaption = AltFromImageTag(imageElement);

                        if (!result.Contains((imageUrl, imageCaption)))
                        {
                            bool meetsCriteria = true;

                            if (imageUrlMustContainOneOfThese != null)
                            {
                                if (!imageUrl.ContainsOneOfThese(imageUrlMustContainOneOfThese))
                                {
                                    meetsCriteria = false;
                                }
                            }

                            if (!imageElementMustContain.IsEmpty())
                            {
                                if (!imageElement.Contains(imageElementMustContain,
                                        StringComparison.InvariantCultureIgnoreCase))
                                {
                                    meetsCriteria = false;
                                }
                            }

                            if (meetsCriteria)
                            {
                                result.Add((imageUrl, imageCaption));
                            }

                        }
                    }
                }

            }

            return result;
        }

        public static (FileInfo fileInfo, int? imageWidth, int? imageHeight) SaveImage(string url, string path)
        {
            try
            {

                FileInfo fileInfo;
                if (File.Exists(path))
                {
                    using (var image = Image.FromFile(path))
                    {
                        fileInfo = new FileInfo(path);
                        return (fileInfo, image.Width, image.Height);
                    }
                }


                WebClient client = new WebClient();
                Stream stream = client.OpenRead(url);
                Bitmap bitmap = new Bitmap(stream);

                if (bitmap != null)
                {
                    bitmap.Save(path, ImageFormat.Png);
                }

                fileInfo = new FileInfo(path);
                var result = (fileInfo, bitmap.Width, bitmap.Height);

                stream.Flush();
                stream.Close();
                client.Dispose();
                bitmap.Dispose();

                return result;

            }
            catch (Exception ex)
            {
                return (null, null, null);
            }


        }
    }
}