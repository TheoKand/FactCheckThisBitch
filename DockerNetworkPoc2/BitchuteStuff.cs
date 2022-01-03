using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DockerNetworkPoc2
{
    public class BitchuteStuff : IDisposable
    {
        private readonly HttpClient _client;
        private readonly string[] _urls;

        public BitchuteStuff(string[] urls)
        {
            var handler = new HttpClientHandler();
            //TODO: If you are using .NET Core 3.0+ you can replace `~DecompressionMethods.None` to `DecompressionMethods.All`
            handler.AutomaticDecompression = ~DecompressionMethods.None;
            _client = new HttpClient(handler);

            _urls = urls;
        }

        public void DoStuff()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.bitchute.com/video/KArcsO2KL7iA/");
            request.Headers.TryAddWithoutValidation("authority", "www.bitchute.com");
            request.Headers.TryAddWithoutValidation("cache-control", "no-store");
            request.Headers.TryAddWithoutValidation("sec-ch-ua", "^^");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "^^");
            request.Headers.TryAddWithoutValidation("upgrade-insecure-requests", "1");
            request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36");
            request.Headers.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
            request.Headers.TryAddWithoutValidation("sec-fetch-mode", "navigate");
            request.Headers.TryAddWithoutValidation("sec-fetch-user", "?1");
            request.Headers.TryAddWithoutValidation("sec-fetch-dest", "document");
            request.Headers.TryAddWithoutValidation("accept-language", "en-GB,en;q=0.9,el;q=0.8");
            request.Headers.TryAddWithoutValidation("cookie", "preferences=^{^%^22theme^%^22:^%^22day^%^22^%^2C^%^22autoplay^%^22:true^}; csrftoken=AZxf69DUm11WStR4JzuxGg8pJ4SkWovCry0xsDzKXpZPCnyBo4URYcNBuWacAN8B; cookielaw=on; __cf_bm=NueUMcYyaMgwFXpKiu9QgC2RuyEDXpmxNypXN9o9eCA-1641223156-0-ARUBFwpwMnUlycLgLFiViSwTc1OBBOXeeCF10j5VJ6g3AhPKZIJ3Ezy819uN7nQ2tt/sG7boMvaOktORAU3gEl3kjXY74oywiJiHXYf0HQU5Fzre9iDZOgILqyq29tIaoQ==");
            var response = _client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            ConsoleTs.WriteLine($"{request.RequestUri.AbsoluteUri} {response.StatusCode} {responseContent.Length}chars");

            request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.bitchute.com/video/kIkNTyD5nPdZ/");
            request.Headers.TryAddWithoutValidation("authority", "www.bitchute.com");
            request.Headers.TryAddWithoutValidation("cache-control", "no-store");
            request.Headers.TryAddWithoutValidation("sec-ch-ua", "^^");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "^^");
            request.Headers.TryAddWithoutValidation("upgrade-insecure-requests", "1");
            request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36");
            request.Headers.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
            request.Headers.TryAddWithoutValidation("sec-fetch-mode", "navigate");
            request.Headers.TryAddWithoutValidation("sec-fetch-user", "?1");
            request.Headers.TryAddWithoutValidation("sec-fetch-dest", "document");
            request.Headers.TryAddWithoutValidation("accept-language", "en-GB,en;q=0.9,el;q=0.8");
            request.Headers.TryAddWithoutValidation("cookie", "preferences=^{^%^22theme^%^22:^%^22day^%^22^%^2C^%^22autoplay^%^22:true^}; csrftoken=AZxf69DUm11WStR4JzuxGg8pJ4SkWovCry0xsDzKXpZPCnyBo4URYcNBuWacAN8B; cookielaw=on; __cf_bm=NueUMcYyaMgwFXpKiu9QgC2RuyEDXpmxNypXN9o9eCA-1641223156-0-ARUBFwpwMnUlycLgLFiViSwTc1OBBOXeeCF10j5VJ6g3AhPKZIJ3Ezy819uN7nQ2tt/sG7boMvaOktORAU3gEl3kjXY74oywiJiHXYf0HQU5Fzre9iDZOgILqyq29tIaoQ==");
            response = _client.SendAsync(request).Result;
            responseContent = response.Content.ReadAsStringAsync().Result;
            ConsoleTs.WriteLine($"{request.RequestUri.AbsoluteUri} {response.StatusCode} {responseContent.Length}chars");

            request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.bitchute.com/video/Jlkoz9VOHszb/");
            request.Headers.TryAddWithoutValidation("authority", "www.bitchute.com");
            request.Headers.TryAddWithoutValidation("cache-control", "no-store");
            request.Headers.TryAddWithoutValidation("sec-ch-ua", "^^");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "^^");
            request.Headers.TryAddWithoutValidation("upgrade-insecure-requests", "1");
            request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36");
            request.Headers.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
            request.Headers.TryAddWithoutValidation("sec-fetch-mode", "navigate");
            request.Headers.TryAddWithoutValidation("sec-fetch-user", "?1");
            request.Headers.TryAddWithoutValidation("sec-fetch-dest", "document");
            request.Headers.TryAddWithoutValidation("accept-language", "en-GB,en;q=0.9,el;q=0.8");
            request.Headers.TryAddWithoutValidation("cookie", "preferences=^{^%^22theme^%^22:^%^22day^%^22^%^2C^%^22autoplay^%^22:true^}; csrftoken=AZxf69DUm11WStR4JzuxGg8pJ4SkWovCry0xsDzKXpZPCnyBo4URYcNBuWacAN8B; cookielaw=on; __cf_bm=akNTlzffEdgMaU1amee6AiA0_aIHWuCt3C_9C83ZZ3k-1641224091-0-AWnDtwHLki5ydYDfgm5noB8NwDj1CNUD2GnHfkwRrTiHW7KPBc7TKZsqs0RqwUafZQ9UHKAv6p/BtdIpvhkiAGByxe0kGfnhEawKmANIiihV703mCjeuPPGoaNGg+7B7Kg==");
            response = _client.SendAsync(request).Result;
            responseContent = response.Content.ReadAsStringAsync().Result;
            ConsoleTs.WriteLine($"{request.RequestUri.AbsoluteUri} {response.StatusCode} {responseContent.Length}chars");

        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}