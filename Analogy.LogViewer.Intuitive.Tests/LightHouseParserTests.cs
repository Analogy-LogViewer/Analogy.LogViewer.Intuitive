#pragma warning disable CA1707
using Analogy.LogViewer.Intuitive.LogsParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analogy.LogViewer.Intuitive.Tests
{
    [TestClass]
    public class LightHouseParserTests
    {
        [TestMethod]
        public async Task TestEventsParser()
        {
            string filename = @"C:\Users\lbanai\Downloads\1490883378792084531.csv";

            using var cancellationTokenSource = new CancellationTokenSource();
            var p = new LightHouseEventsParser();
            MessageHandlerForTesting handler = new MessageHandlerForTesting();
            var allMessages = (await p.Process(filename, cancellationTokenSource.Token, handler)).ToList();
            Assert.IsTrue(allMessages.Any());
        }
        [TestMethod]
        public void Parse_Event_DateTime_Test()
        {
            string time = "2025-11-04 16:26:32.749";
            var parsed = LightHouseEventsParser.ParseDateTime(time);
            var expected = new DateTimeOffset(2025, 11, 4, 16, 26, 32, TimeSpan.Zero).AddMilliseconds(749);
            Assert.IsTrue(parsed.Equals(expected));
        }
        [TestMethod]
        public async Task TestParser()
        {
            string filename = "lighthouse.csv";

            using var cancellationTokenSource = new CancellationTokenSource();
            LightHouseNodeTraceParser p = new LightHouseNodeTraceParser();
            MessageHandlerForTesting handler = new MessageHandlerForTesting();
            var allMessages = (await p.Process(filename, cancellationTokenSource.Token, handler)).ToList();
            Assert.IsTrue(allMessages.Any());
        }
        [TestMethod]
        public void Parse_DateTime_Test()
        {
            string time = "2025-02-19T11:22:13.248837+00:00";
            var parsed = LightHouseNodeTraceParser.ParseDateTime(time);
            var expected = new DateTimeOffset(2025, 02, 19, 11, 22, 13, TimeSpan.Zero).AddMilliseconds(248).AddMicroseconds(837);
            Assert.IsTrue(parsed.Equals(expected));
        }
        [TestMethod]
        public async Task TestEventsJsonParser()
        {
            string filename = "lighthouse-events.json";

            using var cancellationTokenSource = new CancellationTokenSource();
            var p = new LightHouseJsonEventsParser();
            MessageHandlerForTesting handler = new MessageHandlerForTesting();
            var allMessages = (await p.Process(filename, cancellationTokenSource.Token, handler)).ToList();
            Assert.IsTrue(allMessages.Any());
            Assert.AreEqual(2, allMessages.Count);
            Assert.AreEqual(Analogy.Interfaces.DataTypes.AnalogyLogLevel.Error, allMessages[0].Level);
            Assert.IsTrue(allMessages[0].Text!.Contains("SYSTEM_MODE_POWER_ON", StringComparison.Ordinal));
        }
    }
}