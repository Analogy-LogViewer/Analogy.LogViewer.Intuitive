using Analogy.Interfaces.DataTypes;
using System;

namespace Analogy.LogViewer.Intuitive.WinForms.LogsParser
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