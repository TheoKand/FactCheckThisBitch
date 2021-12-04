using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml;
using FackCheckThisBitch.Common;
using Newtonsoft.Json;

namespace FactCheckThisBitch.Admin.Windows
{
    public class UserSettings
    {
        private static UserSettings _instance;
        private static string _settingsFile = Path.Combine(Configuration.Instance().DataFolder, "UserSettings.json");

        protected UserSettings()
        {

        }

        private string _currentPuzzle;
        public string CurrentPuzzle
        {
            get => _currentPuzzle;
            set
            {
                _currentPuzzle = value;
                Save();
            }
        }

        [JsonIgnore]
        public string CurrentPuzzlePath => _currentPuzzle.IsEmpty()
            ? null
            : Path.Combine(Configuration.Instance().DataFolder, _currentPuzzle);

        private bool _puzzleMatchingStrict;
        public bool PuzzleMatchingStrict
        {
            get => _puzzleMatchingStrict;
            set
            {
                _puzzleMatchingStrict = value;
                Save();
            }
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                });
            File.WriteAllTextAsync(_settingsFile, json);
        }

        public static UserSettings Instance()
        {
            _instance ??= ReadSettings();
            return _instance;
        }

        private static UserSettings ReadSettings()
        {
            
            if (File.Exists(_settingsFile))
            {
                var json = File.ReadAllText(_settingsFile);
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
