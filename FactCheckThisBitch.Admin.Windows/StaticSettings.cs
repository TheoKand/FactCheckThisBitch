﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FactCheckThisBitch.Admin.Windows
{
    public static class StaticSettings
    {
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.None
        };
    }
}
