using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
                    result.Add("images", string.Join("\n", images));
                    #endregion
                }
            }

            return result;
        }


        private List<string> GetAdditionalImages(string content)
        {
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
            var images = GetAllMatches(article, imagePatterns, " alt=",new [] {"jpg","png","tiff"});
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

        private List<string> GetAllMatches(string content, string[] patterns, string imageElementMustContain=null, string[] imageUrlMustContainOneOfThese = null)
        {
            var result = new List<string>();
            foreach (var pattern in patterns)
            {
                var matches = Regex.Matches(content, pattern);
                foreach (Match match in matches)
                {
                    if (match.Groups.Count > 1)
                    {
                        var imageElement = match.Groups[0].Value;
                        var imageUrl = match.Groups[1].Value.HtmlDecode();

                        if (!result.Contains(imageUrl))
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
                                result.Add(imageUrl);
                            }

                        }
                    }
                }

            }

            return result;
        }

        public static (FileInfo  fileInfo,int? imageWidth,int? imageHeight) SaveImage(string url, string path)
        {
            FileInfo fileInfo;

            try
            {
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
            catch
            {
                return (null, null, null);
            }

            
        }
    }
}