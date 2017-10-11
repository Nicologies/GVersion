using System.Collections.Generic;
using System.IO;

namespace GVersion
{
    class YamlConfigReader
    {
        public Dictionary<object, object> Settings { get; private set; } = new Dictionary<object, object>();
        public YamlConfigReader(string repoFolder)
        {
            Read(repoFolder, "GitVersionConfig.yaml");
            Read(repoFolder, "GitVersion.yaml");
        }
        private void Read(string repoFolder, string ymlFileName)
        {
            var configFile = Path.Combine(repoFolder, ymlFileName);
            if (!File.Exists(configFile))
            {
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
