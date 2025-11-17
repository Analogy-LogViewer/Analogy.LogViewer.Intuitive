using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Parsing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

namespace MediaManager.Logging
{
    [ExcludeFromCodeCoverage]
#if NET
    [SupportedOSPlatform("windows")]
#endif
    public class EncryptSink : ILogEventSink, IDisposable
    {
        private readonly ILogEventSink sink;
        private readonly ITextFormatter? textFormatter;
        private readonly EncryptionLogic encryptionLogic;
        private readonly MessageTemplateParser parser = new MessageTemplateParser();
        private readonly StringWriter textWriter = new StringWriter();
        private readonly object sync;
        private bool disposed;
        private EncryptSink(EncryptionLogic encryptionLogic, ILogEventSink sink)
        {
            this.sink = sink ?? throw new ArgumentNullException(nameof(sink));
            this.encryptionLogic = encryptionLogic;
            sync = new object();
        }
        public EncryptSink(EncryptionLogic encryptionLogic, ILogEventSink sink, string outputTemplate, IFormatProvider? formatProvider) : this(encryptionLogic, sink)
        {
            textFormatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
        }

        public EncryptSink(EncryptionLogic encryptionLogic, ILogEventSink sink, ITextFormatter formatProvider) : this(encryptionLogic, sink)
        {
            textFormatter = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            try
            {
                lock (sync)
                {
                    if (disposed)
                    {
                        return;
                    }
                    textFormatter?.Format(logEvent, textWriter);
                    textWriter.Flush();
                    var message = textWriter.GetStringBuilder().ToString();
                    textWriter.GetStringBuilder().Clear();
                    var encryptedMessage = encryptionLogic.EncryptLogEntry(message);
                    var template = parser.Parse(encryptedMessage);

                    var encryptedLogEvent = new LogEvent(
                        logEvent.Timestamp,
                        logEvent.Level,
                        logEvent.Exception,
                        new MessageTemplate(encryptedMessage, template.Tokens),
                        logEvent.Properties
                            .Select(kvp => new LogEventProperty(kvp.Key, kvp.Value)));
                    if (!disposed)
                    {
                        sink.Emit(encryptedLogEvent);
                    }
                }
            }
            catch (Exception e)
            {
                // Logger class could be disposed before calling the above emit method.
                // so in any case the message will be lost.
                //Logger is disposed right before the application is closed.
                Console.WriteLine(e);
            }
        }

        public void Dispose()
        {
            try
            {
                disposed = true;
                if (sink is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                textWriter.Dispose();
            }
            catch
            {
                //do nothing - we are during shutdown
            }
        }
    }
}