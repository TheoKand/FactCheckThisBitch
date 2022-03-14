using System.IO;
using FackCheckThisBitch.Common;
using Newtonsoft.Json;

namespace VideoFromArticle.Admin.Windows
{
    public class UserSettings
    {
        private static UserSettings _instance;
        private static readonly string SettingsFile = Path.Combine(Configuration.Instance().DataFolder, "UserSettings.json");

        protected UserSettings()
        {

        }

        private string _currentFile;
        public string CurrentFile
        {
            get => _currentFile;
            set
            {
                _currentFile = value;
                Save();
            }
        }

        private string _narrationOptionsVoice;
        public string NarrationOptionsVoice
        {
            get => _narrationOptionsVoice;
            set
            {
                _narrationOptionsVoice = value;
                Save();
            }
        }

        [JsonIgnore]
        public string CurrentFilePath => _currentFile.IsEmpty()
            ? null
            : Path.Combine(Configuration.Instance().DataFolder, _currentFile);


        private void Save()
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
            _instance ??= ReadSettings();
            return _instance;
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
