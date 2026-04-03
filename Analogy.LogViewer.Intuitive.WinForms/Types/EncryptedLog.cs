using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.File;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace MediaManager.Logging
{
    [ExcludeFromCodeCoverage]
#if NET
    [SupportedOSPlatform("windows")]
#endif
    public static class EncryptedLog
    {
        public static LoggerConfiguration EncryptToFile(
            this LoggerSinkConfiguration loggerConfiguration,
            string filePath,
            EncryptionLogic encryptionLogic,
            long? fileSizeLimitBytes = null,
            LoggingLevelSwitch? levelSwitch = null,
            TimeSpan? flashToDiskInterval = null,
            bool buffered = false,
            RollingInterval rollingInterval = RollingInterval.Hour,
            string outputTemplate =
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception} {Properties:j}",
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
            IFormatProvider? formatProvider = null,
            bool shared = false,
            FileLifecycleHooks? hooks = null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return LoggerSinkConfiguration.Wrap(loggerConfiguration, sink =>
                new EncryptSink(encryptionLogic, sink, outputTemplate, formatProvider), writeTo =>
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    writeTo.File(filePath, restrictedToMinimumLevel, "{Message:lj}{NewLine}", formatProvider,
                        fileSizeLimitBytes: fileSizeLimitBytes, levelSwitch: levelSwitch, buffered: buffered, shared,
                        flushToDiskInterval: flashToDiskInterval, rollingInterval, hooks: hooks);
                }
            }, LevelAlias.Minimum, levelSwitch: null);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public static LoggerConfiguration EncryptToFile(
            this LoggerSinkConfiguration loggerConfiguration,
            ITextFormatter formatter,
            string filePath,
            EncryptionLogic encryptionLogic,
            long? fileSizeLimitBytes = null,
            LoggingLevelSwitch? levelSwitch = null,
            TimeSpan? flashToDiskInterval = null,
            bool buffered = false,
            RollingInterval rollingInterval = RollingInterval.Hour,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
            bool shared = false,
            FileLifecycleHooks? hooks = null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return LoggerSinkConfiguration.Wrap(loggerConfiguration, sink =>
                new EncryptSink(encryptionLogic, sink, formatter), writeTo =>
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    writeTo.File(filePath, restrictedToMinimumLevel, "{Message:lj}{NewLine}", formatProvider: null,
                        fileSizeLimitBytes: fileSizeLimitBytes, levelSwitch: levelSwitch, buffered: buffered, shared,
                        flushToDiskInterval: flashToDiskInterval, rollingInterval, hooks: hooks);
                }
            }, LevelAlias.Minimum, levelSwitch);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public static LoggerConfiguration EncryptToSink(
            this LoggerSinkConfiguration loggerConfiguration,
            ITextFormatter formatter,
            EncryptionLogic encryptionLogic,
            ILogEventSink destinationSink,
            LoggingLevelSwitch? levelSwitch = null,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return LoggerSinkConfiguration.Wrap(loggerConfiguration, sink =>
                    new EncryptSink(encryptionLogic, sink, formatter),
                writeTo => writeTo.Sink(destinationSink, restrictedToMinimumLevel, levelSwitch), LevelAlias.Minimum,
                levelSwitch);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public static LoggerConfiguration EncryptToSink(
            this LoggerSinkConfiguration loggerConfiguration,
            string outputTemplate,
            EncryptionLogic encryptionLogic,
            ILogEventSink destinationSink,
            LoggingLevelSwitch? levelSwitch = null,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return LoggerSinkConfiguration.Wrap(loggerConfiguration, sink =>
                    new EncryptSink(encryptionLogic, sink, outputTemplate, formatProvider: null),
                writeTo => writeTo.Sink(destinationSink, restrictedToMinimumLevel, levelSwitch), LevelAlias.Minimum,
                levelSwitch);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}