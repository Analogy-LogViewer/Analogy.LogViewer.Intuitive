using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.Loaders;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.WinForms.LogsParser
{
    internal sealed class V2CombinedEncryptedAndNonEncryptedLog(Guid id) : BaseCombinedEncryptedAndNonEncryptedLog
    {
        public override string? OptionalTitle { get; set; } = "MMA V2.X logs";

        public sealed override Guid Id { get; set; } = id;
        private LegacyParser Parser { get; set; } = null!;

        public override Task InitializeDataProvider(ILogger logger)
        {
            Parser = new LegacyParser();
            return base.InitializeDataProvider(logger);
        }

        protected override IAnalogyLogMessage ParseMessage(string logLine) => Parser.Parse(logLine);
    }
}