using System;
using System.Collections.Generic;
using System.IO;
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
                {"title", new[] {"title"}},
                {"description", new[] {"description"}},
                {"author", new[] {"author"}},
                {"site_name", new[] {"site_name"}},
                {"original-source", new[] {"original-source"}},
                {"datePublished", new[] {"datePublished", "published", "published_time"}},
                {"keywords", new[] {"keywords"}},
                {"image", new[] {"image"}}
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
                    string html = null; //await content.ReadAsStringAsync();

                    using (var sr = new StreamReader(await content.ReadAsStreamAsync(), Encoding.GetEncoding("utf-8")))
                    {
                        html = await sr.ReadToEndAsync();
                    }


                    foreach (string propertyName in MetadataProperties.Keys)
                    {
                        foreach (string possibleMeta in MetadataProperties[propertyName])
                        {
                            var propertyValue = TryGetMetadataProperty(html, propertyName);
                            if (!propertyValue.IsEmpty())
                            {
                                if (!result.ContainsKey(propertyName))
                                {
                                    result.Add(propertyName, propertyValue);
                                }
                            }
                        }
                    }
                }
            }


            return result;
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

            if (content.Contains("Boy, 11, who died suddenly in his sleep to be given 'funeral he deserves'"))
            {
                Console.WriteLine("stop");
            }
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
    }
}