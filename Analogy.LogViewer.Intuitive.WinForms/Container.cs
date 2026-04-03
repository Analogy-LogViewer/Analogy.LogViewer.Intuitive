using Analogy.LogViewer.Intuitive.Types;
using MediaManager.Logging;
using System;

namespace Analogy.LogViewer.Intuitive
{
    public class Container
    {
#pragma warning disable SA1311
        private static readonly Lazy<Container> instance = new Lazy<Container>(() => new Container());
#pragma warning restore SA1311
        public static Container Instance => instance.Value;

        public EncryptionLogic EncryptionLogic { get; set; }

        private Container()
        {
            EncryptionLogic = new EncryptionLogic(SerilogEncryptedLogFileMode.ReadLogs, "Analogy_Key");
        }
    }
}