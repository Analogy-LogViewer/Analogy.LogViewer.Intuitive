using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.ElasticCommonSchema.Parsers;
using Analogy.LogViewer.Intuitive.Loaders;
using Analogy.LogViewer.Intuitive.Managers;
using DevExpress.XtraCharts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.LogsParser
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