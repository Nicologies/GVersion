using System.Collections.Generic;
using System.IO;

namespace GVersion
{
    class SettingsProvider
    {
        public static SettingsProvider Instance = new SettingsProvider();
        public Dictionary<object, object> Settings { get; private set; }
        private SettingsProvider()
        {
            if (!File.Exists("GitVersionConfig.yaml"))
            {
                Settings = new Dictionary<object, object>();
                return;
            } 
            var yaml = new YamlDotNet.Serialization.Deserializer();
            using (var reader = new StreamReader("GitVersionConfig.yaml"))
            {
                Settings = yaml.Deserialize<Dictionary<object, object>>(reader);
            }
        }
    }
}
