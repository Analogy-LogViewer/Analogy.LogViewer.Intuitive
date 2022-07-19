using System;

namespace Analogy.LogViewer.Intuitive
{
    [Serializable]
    public class UserSettings
    {
        public string GroupId { get; set; }
        public string KafkaAddress { get; set; }
        public string KafkaTopic { get; set; }
        public UserSettings()
        {
            GroupId = "AnalogyKafka";
            KafkaTopic = "NlogKafka_Logging";
            KafkaAddress = "localhost:9092";
        }
    }
}
