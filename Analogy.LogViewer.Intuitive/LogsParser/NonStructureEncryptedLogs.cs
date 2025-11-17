using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using System;

namespace Analogy.LogViewer.Intuitive.LogsParser
{
    public class NonStructureEncryptedLogs : BaseEncryptedLog
    {
        public override string? OptionalTitle { get; set; } = "Generic Non Structured Encrypted logs";
        public override Guid Id { get; set; } = new Guid("69f19155-a4f2-4831-ae4a-90d2987e8322");
        protected override IAnalogyLogMessage ParseMessage(string logLine) => new AnalogyInformationMessage(logLine) { Level = AnalogyLogLevel.Unknown };
    }
}