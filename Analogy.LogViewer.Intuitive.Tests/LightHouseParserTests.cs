#pragma warning disable CA1707
using Analogy.LogViewer.Intuitive.LogsParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analogy.LogViewer.Intuitive.Tests
{
    [TestClass]
    public class LightHouseParserTests
    {
        //[TestMethod]
        //public async Task TestEventsParser()
        //{
        //    string filename = @"C:\Users\lior.banai.LIORB-DELL\Downloads\events_690a24e9-39de-49b0-9754-73a98c5ccceb.19a9babccaf.csv";

        //    using var cancellationTokenSource = new CancellationTokenSource();
        //    var p = new LightHouseEventsParser();
        //    MessageHandlerForTesting handler = new MessageHandlerForTesting();
        //    var allMessages = (await p.Process(filename, cancellationTokenSource.Token, handler)).ToList();
        //    Assert.IsTrue(allMessages.Any());
        //}
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
    }
}