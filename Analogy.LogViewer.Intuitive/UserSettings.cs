using System;

namespace Analogy.LogViewer.Intuitive
{
    [Serializable]
    public class UserSettings
    {
        public string KeyContainerNameRSA { get; set; }

        public UserSettings()
        {
            KeyContainerNameRSA = "OrpheusFullKeyContainerRSA";
        }
    }
}
