using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using FackCheckThisBitch.Common;
using Newtonsoft.Json;
using VideoFromArticle.Models;

namespace VideoFromArticle.Admin.Windows
{
    public class UserSettings
    {
        private static readonly object lockObject = new object();

        private static UserSettings _instance;
        private static readonly string SettingsFile = Path.Combine(Configuration.Instance().DataFolder, "UserSettings.json");

        protected UserSettings()
        {

        }

        private Slideshow _currentSlideshow;
        public Slideshow CurrentSlideshow
        {
            get => _currentSlideshow;
            set
            {
                _currentSlideshow = value;
            }
        }

        private string _narrationOptionsVoice;
        public string NarrationOptionsVoice
        {
            get => _narrationOptionsVoice;
            set
            {
                _narrationOptionsVoice = value;
            }
        }

        private string _renderOptionsTemplate;
        public string RenderOptionsTemplate
        {
            get => _renderOptionsTemplate;
            set
            {
                _renderOptionsTemplate = value;
            }
        }

        private int _renderOptionsIntroDurationSeconds;
        public int RenderOptionsIntroDuration
        {
            get => _renderOptionsIntroDurationSeconds;
            set
            {
                _renderOptionsIntroDurationSeconds = value;
            }
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                });
            File.WriteAllTextAsync(SettingsFile, json);
        }

        public static UserSettings Instance()
        {
            lock (lockObject)
            {
                _instance ??= ReadSettings();
                return _instance;
            }

            
            
        }

        private static UserSettings ReadSettings()
        {

            if (File.Exists(SettingsFile))
            {
                var json = File.ReadAllText(SettingsFile);
                var userSettings = JsonConvert.DeserializeObject<UserSettings>(json);
                return userSettings;
            }
            else
            {
                return new UserSettings();
            }

        }
    }
}
