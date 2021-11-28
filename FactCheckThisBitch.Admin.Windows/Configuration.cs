using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace FactCheckThisBitch.Admin.Windows
{
    public class Configuration
    {
        private static Configuration _instance;

        protected Configuration() {}

        public string DataFolder => ConfigurationManager.AppSettings.Get("DataFolder");

        public static Configuration Instance()
        {
            _instance = _instance ?? new Configuration();
            return _instance;
        }
    }
}
