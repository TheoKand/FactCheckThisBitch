using System.Collections.Generic;
using Newtonsoft.Json;

namespace VideoFromArticle.Admin.Windows
{
    public static class StaticSettings
    {
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static Dictionary<string, string> AvailableVoices = new Dictionary<string, string>()
        {
            { "ttsVoiceen-US-AriaNeural", "Grace, Female" },
            { "uniform-ttsVoiceen-US-GuyNeural", "Andrew, Male" }
            //, { "ttsVoiceJoanna", "Joanna, Female" }
        };

        public static long MinimumArticleImageSize = 150 * 1024;
        public static long MinimumArticleImageWidth = 448;

        public static long MinimumArticleImageHeight = 336;
        //public static long MaximumArticleImageCount = 50;
    }
}