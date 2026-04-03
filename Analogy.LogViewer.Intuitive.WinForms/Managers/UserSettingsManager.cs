using System;
using System.Collections.Generic;
using System.IO;

namespace Analogy.LogViewer.Intuitive.Managers
{
    [Serializable]
    public class UserSettingsManager
    {
        public static UserSettingsManager Instance { get; } = new Lazy<UserSettingsManager>(() => new UserSettingsManager()).Value;
        public string FileName => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Analogy.LogViewer", "Analogy.Intuitive.Settings.json");

        public string SubscribePort
        {
            get => Settings.SubscribePort;
            set => Settings.SubscribePort = value;
        }

        public string PublishPort
        {
            get => Settings.PublishPort;
            set => Settings.PublishPort = value;
        }
        public string FFmpegBinaryFolder
        {
            get => Settings.FFmpegBinaryFolder;
            set => Settings.FFmpegBinaryFolder = value;
        }
        public string LastVideoFileLoaded
        {
            get => Settings.LastVideoFileLoaded;
            set => Settings.LastVideoFileLoaded = value;
        }

        public bool ShowAllColumnsFromMetaDataField
        {
            get => Settings.ShowAllColumnsFromMetaDataField;
            set => Settings.ShowAllColumnsFromMetaDataField = value;
        }

        public List<string> AdditionalColumnsFromMetaDataField
        {
            get => Settings.AdditionalColumnsFromMetaDataField;
            set => Settings.AdditionalColumnsFromMetaDataField = value;
        }
        public string FileOpenDialogFilters
        {
            get => Settings.FileOpenDialogFilters;
            set => Settings.FileOpenDialogFilters = value;
        }
        public List<string> SupportFormats
        {
            get => Settings.SupportFormats;
            set => Settings.SupportFormats = value;
        }
        private UserSettings Settings { get; }
        public UserSettingsManager()
        {
            if (File.Exists(FileName))
            {
                try
                {
                    Settings = Utils.DeSerializeJsonFile<UserSettings>(FileName);
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
            try
            {
                Utils.SerializeToJsonFile(Settings, FileName);
            }
            catch
            {
                //ignore error
            }
        }
    }
}