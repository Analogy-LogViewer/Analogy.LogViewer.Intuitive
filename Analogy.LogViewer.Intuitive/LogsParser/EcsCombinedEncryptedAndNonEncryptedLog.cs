using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.ElasticCommonSchema.Parsers;
using Analogy.LogViewer.Intuitive.Managers;
using System;

namespace Analogy.LogViewer.Intuitive.LogsParser
{
    public class EcsCombinedEncryptedAndNonEncryptedLog : BaseCombinedEncryptedAndNonEncryptedLog
    {
        public override string? OptionalTitle { get; set; } = "Common ECS Logs (Encrypted and Non Encrypted logs)";
        public sealed override Guid Id { get; set; }

        public EcsCombinedEncryptedAndNonEncryptedLog(Guid id)
        {
            Id = id;
        }
        protected override IAnalogyLogMessage ParseMessage(string logLine) => EcsDocumentUtils.ParseLine(logLine,
            UserSettingsManager.Instance.ShowAllColumnsFromMetaDataField,
            UserSettingsManager.Instance.AdditionalColumnsFromMetaDataField);
    }
}