using System.Collections.Generic;
using System.IO;

namespace GVersion
{
    class YamlConfigReader
    {
        public Dictionary<object, object> Settings { get; private set; }
        public YamlConfigReader(string repoFolder)
        {
            var configFile = Path.Combine(repoFolder, "GitVersionConfig.yaml");
            if (!File.Exists(configFile))
            {
                Settings = new Dictionary<object, object>();
                return;
            }
            var yaml = new YamlDotNet.Serialization.Deserializer();
            using (var reader = new StreamReader(configFile))
            {
                Settings = yaml.Deserialize<Dictionary<object, object>>(reader);
            }
        }
    }
}
