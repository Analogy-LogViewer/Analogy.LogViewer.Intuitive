using Analogy.LogViewer.Intuitive.LogsParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analogy.LogViewer.Intuitive.Tests
{
    [TestClass]
    public sealed class ElasticCsvDataProviderTests
    {
        [TestMethod]
        public async Task TestParser()
        {
            string filename = "log.csv";

            using var cancellationTokenSource = new CancellationTokenSource();
            ElasticCsvDataProvider p = new ElasticCsvDataProvider();
            MessageHandlerForTesting handler = new MessageHandlerForTesting();
            var allMessages = (await p.Process(filename, cancellationTokenSource.Token, handler)).ToList();
            Assert.Contains("Start new recording", allMessages[1].Text!, StringComparison.Ordinal);
        }

        [TestMethod]
        public void DateTime1ParserTest()
        {
            string dateTimeStr = "Mar 7, 2025 @ 19:47:27.759";
            var parsed = ElasticCsvDataProvider.ParseDateTime(dateTimeStr);
            var expected = new DateTimeOffset(2025, 03, 07, 19, 47, 27, TimeSpan.Zero).AddMilliseconds(759);
            Assert.IsTrue(parsed.Equals(expected));
        }
        [TestMethod]
        public void DateTime2ParserTest()
        {
            string dateTimeStr = "Mar 19, 2025 @ 06:30:00.059+0000";
            var parsed = ElasticCsvDataProvider.ParseDateTime(dateTimeStr);
            var expected = new DateTimeOffset(2025, 03, 19, 06, 30, 00, TimeSpan.Zero).AddMilliseconds(59);
            Assert.IsTrue(parsed.Equals(expected));
        }
    }
}