using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace DockerNetworkPoc2
{
    public class NetworkStuff
    {
        public static (string ip, string ipLocation) GetIpInfo()
        {

            var whatsMyIpPageContent = QueryWhatsMyIp("https://www.whatismyip.com/");


            var (ip, ipLocation) = ParseWhatsMyIp(whatsMyIpPageContent);

            return (ip, ipLocation);
        }

        private static string QueryWhatsMyIp(string url)
        {
            var handler = new HttpClientHandler();
            string response;

            // If you are using .NET Core 3.0+ you can replace `~DecompressionMethods.None` to `DecompressionMethods.All`
            handler.AutomaticDecompression = ~DecompressionMethods.None;

            // In production code, don't destroy the HttpClient through using, but better reuse an existing instance
            // https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.whatismyip.com/"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "www.whatismyip.com");
                    request.Headers.TryAddWithoutValidation("cache-control", "max-age=0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua", "^^");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "^^");
                    request.Headers.TryAddWithoutValidation("upgrade-insecure-requests", "1");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "cross-site");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "navigate");
                    request.Headers.TryAddWithoutValidation("sec-fetch-user", "?1");
                    request.Headers.TryAddWithoutValidation("sec-fetch-dest", "document");
                    request.Headers.TryAddWithoutValidation("referer", "https://www.google.com/");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-GB,en;q=0.9,el;q=0.8");
                    request.Headers.TryAddWithoutValidation("cookie", "_hjSessionUser_2252679=eyJpZCI6IjY0NmYwZDI3LTQ5NmItNTE0Ni05ZDYyLTYwMDgzY2Q5MDQ4ZCIsImNyZWF0ZWQiOjE2NDA4MTM4MDM4MTcsImV4aXN0aW5nIjp0cnVlfQ==; _gid=GA1.2.1179580015.1641132749; _hjSession_2252679=eyJpZCI6ImQ5MTdiMjA0LTIzMDktNDc4ZS1hNDlmLWNmNDc1MDU3ZmUyNyIsImNyZWF0ZWQiOjE2NDExMzI3NDg3OTl9; _hjAbsoluteSessionInProgress=0; __cf_bm=L5nPDdZrUoDIIpiwHcEBA_.k7km0C6e_hgXRAkJH7Mk-1641132780-0-AdawV4MCnrHkdoLqsE60AONq+yLpHZ9ZNkfox8BFaD/Pi4YDEX/gWPmCkXmvN15WTHS7wP41deNs1BoyOtPdyGLIU0pS8a9VI2IOTo8xUOxyKkLtQS/P/t9M9Us821dq8A==; _dc_gtm_UA-4975660-1=1; _hjIncludedInPageviewSample=1; _hjIncludedInSessionSample=0; _ga_4T6GFV4RYJ=GS1.1.1641132748.2.1.1641133344.0; _ga=GA1.2.1160371766.1640813804");

                    response = httpClient.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
                }
            }

            return response;

        }

        private static (string ip, string ipLocation) ParseWhatsMyIp(string content)
        {
            string ipPattern =
                "id=\"ipv4\"[^<]+>([^<]+)";
            string ipLocationPattern = ">My IP Location: ([^<]+)";

            var ipMatch = Regex.Match(content, ipPattern);
            var ip = ipMatch.Groups[1].Value;

            var ipLocationMatch = Regex.Match(content, ipLocationPattern);
            var ipLocation = ipLocationMatch.Groups[1].Value;

            return (ip, ipLocation);
        }


    }
}
