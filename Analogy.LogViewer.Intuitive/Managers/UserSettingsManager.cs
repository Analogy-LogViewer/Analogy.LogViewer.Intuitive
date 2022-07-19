using System;
using System.IO;

namespace Analogy.LogViewer.Intuitive.Managers
{
    [Serializable]
    public class UserSettingsManager
    {
        private static readonly Lazy<UserSettingsManager> _instance =
            new Lazy<UserSettingsManager>(() => new UserSettingsManager());
        public static UserSettingsManager Instance { get; } = _instance.Value;
        public string FileName { get; } = "IntuitiveSettings.dat";
        public UserSettings Settings { get; }

        public UserSettingsManager()
        {
            if (File.Exists(FileName))
            {
                try
                {
                    Settings = Utils.DeSerializeBinaryFile<UserSettings>(FileName);

                }
                catch (Exception)
                {
                    Settings = new UserSettings();
                }
            }
            else
            {
                Settings = new UserSettings();
            }
        }
        public void Save()
        {
            Utils.SerializeToBinaryFile(Settings, FileName);
        }
    }
}
