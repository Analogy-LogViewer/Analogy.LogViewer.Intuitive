using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.Loaders;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.LogsParser
{
    public class V2EncryptedLogs : BaseEncryptedLog
    {
        public override string? OptionalTitle { get; set; } = "MMA V2.X Encrypted logs";
        public override Guid Id { get; set; } = new Guid("f7cf5d31-5e89-4206-b691-79c6d8cf8578");
        private LegacyParser Parser { get; set; } = null!;

        public override Task InitializeDataProvider(ILogger logger)
        {
            Parser = new LegacyParser();
            return base.InitializeDataProvider(logger);
        }

        protected override IAnalogyLogMessage ParseMessage(string logLine) => Parser.Parse(logLine);
    }
}