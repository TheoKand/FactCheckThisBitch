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

            request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.youtube.com/watch?v=mqL7aE_-naM&t=1s");
            request.Headers.TryAddWithoutValidation("authority", "www.youtube.com");
            request.Headers.TryAddWithoutValidation("cache-control", "max-age=0");
            request.Headers.TryAddWithoutValidation("sec-ch-ua", "^^");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "^^");
            request.Headers.TryAddWithoutValidation("upgrade-insecure-requests", "1");
            request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36");
            request.Headers.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.Headers.TryAddWithoutValidation("service-worker-navigation-preload", "true");
            request.Headers.TryAddWithoutValidation("x-client-data", "CIS2yQEIorbJAQjBtskBCKmdygEIkP7KAQjr8ssBCKf5ywEI1vzLAQjmhMwBCLWFzAEIy4nMAQitjswBCNKPzAEI2pDMARiqqcoBGIyeywE=");
            request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
            request.Headers.TryAddWithoutValidation("sec-fetch-mode", "navigate");
            request.Headers.TryAddWithoutValidation("sec-fetch-user", "?1");
            request.Headers.TryAddWithoutValidation("sec-fetch-dest", "document");
            request.Headers.TryAddWithoutValidation("accept-language", "en-GB,en;q=0.9,el;q=0.8");
            request.Headers.TryAddWithoutValidation("cookie", "YSC=wN3yaT91RwA; CONSENT=PENDING+200; SID=FwiAzDqyB99KzGSEG-oKV--t-xHQ1JIOwt2_liwHD_5ofrZg0iNhiPndspFEhMJ49TvPvg.; __Secure-1PSID=FwiAzDqyB99KzGSEG-oKV--t-xHQ1JIOwt2_liwHD_5ofrZgi5w-FeI918iEd8MmXp_HZg.; __Secure-3PSID=FwiAzDqyB99KzGSEG-oKV--t-xHQ1JIOwt2_liwHD_5ofrZgomMoIzQBuQZX1BuHaRaALQ.; HSID=AtaSk_UXJIgKkF3SA; SSID=APztOVty2m0eUxsJn; APISID=HhiaanbzlPfBychs/AQORZ_r-EDurqODyA; SAPISID=JKayrQ97cAZJ0xaZ/Af4mEVyRLLCxvLogt; __Secure-1PAPISID=JKayrQ97cAZJ0xaZ/Af4mEVyRLLCxvLogt; __Secure-3PAPISID=JKayrQ97cAZJ0xaZ/Af4mEVyRLLCxvLogt; LOGIN_INFO=AFmmF2swRQIhAJNVScaN-iyXVyb7ijc5kgamrAQNdi5mQjeHkSckIhi1AiAK1KEhMvl-Dh-ZE_gRyc4_5lKZxGaFxkeUIoE71yYfNQ:QUQ3MjNmejFjcE82TkxMUXpCRkFWYWVhM1VrcG1HblN2eDJWcXI2Z2x4R2w2akpnWVIyRDF0WXE3eEszMkVES0x3c2F3SGt0RXVzTlpQcVVGUDhGRzZpN1pqMExjRDNpWUNiNU1sX2g0ektiV1ZCSHRobWJ4X0lNaHIxU1VuX0REX045NkV2Q2dsZmJvbURkUDdQUi1VTmc3U1QxX2xHaW4ydVpLclNaQjRJT1NnRTVLbk0zZmdIV3FsUUVCNUpYak44VkI2RW1KQllfTk9QR0k2V1VOeTlSSDgzQTJWVUVlZw==; VISITOR_INFO1_LIVE=CyZki_6cmOs; PREF=tz=Europe.London&f6=40000400; SIDCC=AJi4QfGGIBQtj3aKPxYLn5dcaeyhe7ccQ9LFdb2ARqNEwjSlOEBsaDxERDuLFmcGFohq5MrgPQ; __Secure-3PSIDCC=AJi4QfG4tUnFpmRq59IHza2TKg_J-k10Ymzw9Fwa2Eb7-xdmYG1LXSwqLIzw7yLj4jBYDfKlTjs");
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