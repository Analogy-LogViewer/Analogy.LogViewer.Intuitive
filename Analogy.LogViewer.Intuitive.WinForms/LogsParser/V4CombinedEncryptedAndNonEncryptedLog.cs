using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.ElasticCommonSchema.Parsers;
using Analogy.LogViewer.Intuitive.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.LogsParser
{
    internal sealed class V4CombinedEncryptedAndNonEncryptedLog : BaseCombinedEncryptedAndNonEncryptedLog
    {
        public override string? OptionalTitle { get; set; } = "MMA ECS logs";
        public sealed override Guid Id { get; set; }
        public V4CombinedEncryptedAndNonEncryptedLog(Guid id)
        {
            Id = id;
        }
        protected override IAnalogyLogMessage ParseMessage(string logLine)
            => EcsDocumentUtils.ParseLine(logLine,
                    UserSettingsManager.Instance.ShowAllColumnsFromMetaDataField,
                    UserSettingsManager.Instance.AdditionalColumnsFromMetaDataField);
    }
}