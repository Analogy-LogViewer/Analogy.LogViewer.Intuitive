using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.ElasticCommonSchema.Parsers;
using Analogy.LogViewer.Intuitive.Managers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.LogsParser
{
    public class V4EncryptedLogs : BaseEncryptedLog
    {
        public override string? OptionalTitle { get; set; } = "MMA Encrypted logs";
        public override Guid Id { get; set; } = new Guid("ef331632-b01b-4ed7-a77f-d35b23ae6cbe");
        protected override IAnalogyLogMessage ParseMessage(string logLine)
            =>
            EcsDocumentUtils.ParseLine(logLine,
            UserSettingsManager.Instance.ShowAllColumnsFromMetaDataField,
            UserSettingsManager.Instance.AdditionalColumnsFromMetaDataField);
    }
}