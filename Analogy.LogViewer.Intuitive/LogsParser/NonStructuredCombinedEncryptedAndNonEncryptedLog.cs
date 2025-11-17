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
    internal sealed class NonStructuredCombinedEncryptedAndNonEncryptedLog : BaseCombinedEncryptedAndNonEncryptedLog
    {
        public override string? OptionalTitle { get; set; } = "Generic Non Structured logs";
        public sealed override Guid Id { get; set; }
        protected override IAnalogyLogMessage ParseMessage(string logLine) => new AnalogyInformationMessage(logLine) { Level = AnalogyLogLevel.Unknown };

        public NonStructuredCombinedEncryptedAndNonEncryptedLog(Guid id)
        {
            Id = id;
        }
    }
}