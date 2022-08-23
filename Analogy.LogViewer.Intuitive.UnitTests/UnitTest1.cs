using System.Threading;
using System.Threading.Tasks;
using Analogy.LogViewer.Intuitive.IAnalogy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analogy.LogViewer.Intuitive.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestLegacyFile()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = "20220823.log";
            LegacyFileLoader parser = new LegacyFileLoader();
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = await parser.Process(fileName, cts.Token, forTesting);

        }
    }
}
