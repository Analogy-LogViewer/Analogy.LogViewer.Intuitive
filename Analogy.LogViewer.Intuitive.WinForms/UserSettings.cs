using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Intuitive
{
    [Serializable]
    public class UserSettings
    {
        public string SubscribePort { get; set; }
        public string PublishPort { get; set; }
        public string FFmpegBinaryFolder { get; set; }
        public string LastVideoFileLoaded { get; set; }
        public bool ShowAllColumnsFromMetaDataField { get; set; }
        public List<string> AdditionalColumnsFromMetaDataField { get; set; }
        public string FileOpenDialogFilters { get; set; }
        public List<string> SupportFormats { get; set; }
        public UserSettings()
        {
            FFmpegBinaryFolder = "";
            LastVideoFileLoaded = "";
            ShowAllColumnsFromMetaDataField = true;
            AdditionalColumnsFromMetaDataField = new List<string>() { };
            FileOpenDialogFilters = "All Supported formats (*.Clef;*.log;*.gz)|*.clef;*.log;*.gz|Clef format (*.clef)|*.clef|Plain log text file (*.log)|*.log|GZIP file (*.gz)|*.gz";
            SupportFormats = new List<string> { "*.Clef", "*.log", "*.gz" };
            SubscribePort = ">tcp://localhost:8000";
            PublishPort = ">tcp://localhost:8001";
        }
    }
}