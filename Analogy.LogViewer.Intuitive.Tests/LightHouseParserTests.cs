#pragma warning disable CA1707
using Analogy.LogViewer.Intuitive.LogsParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analogy.LogViewer.Intuitive.Tests
{
    [TestClass]
    public class LightHouseParserTests
    {
        [TestMethod]
        public async Task TestParser()
        {
            string filename = "lighthouse.csv";

            using var cancellationTokenSource = new CancellationTokenSource();
            LightHouseParser p = new LightHouseParser();
            MessageHandlerForTesting handler = new MessageHandlerForTesting();
            var allMessages = (await p.Process(filename, cancellationTokenSource.Token, handler)).ToList();
            Assert.IsTrue(allMessages.Any());
        }
        [TestMethod]
        public void Parse_DateTime_Test()
        {
            string time = "2025-02-19T11:22:13.248837+00:00";
            var parsed = LightHouseParser.ParseDateTime(time);
            var expected = new DateTimeOffset(2025, 02, 19, 11, 22, 13, TimeSpan.Zero).AddMilliseconds(248).AddMicroseconds(837);
            Assert.IsTrue(parsed.Equals(expected));
        }
    }
}