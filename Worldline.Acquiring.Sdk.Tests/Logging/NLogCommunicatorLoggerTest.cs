using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;

namespace Worldline.Acquiring.Sdk.Logging
{
    [TestFixture]
    public class NLogCommunicatorLoggerTest
    {
        [Test]
        public void TestLog()
        {
            var mock = new Mock<ILogger>();
            var messageList = new List<Tuple<LogLevel, string>>();
            mock.Setup(t => t.Log(It.IsAny<LogLevel>(), It.IsAny<string>())).Callback((LogLevel logLevel, string message) => messageList.Add(Tuple.Create(logLevel,message)));
            var communicatorLogger = new NLogCommunicatorLogger(mock.Object, LogLevel.Info, LogLevel.Warn);
            communicatorLogger.Log("Hello world");

            Assert.That(messageList, Has.Count.EqualTo(1));

            var record = messageList.First();
            Assert.That(record.Item2, Is.EqualTo("Hello world"));
            Assert.That(record.Item1, Is.EqualTo(LogLevel.Info));
        }

        [Test]
        public void TestLogException()
        {
            var mock = new Mock<ILogger>();
            var messageList = new List<Tuple<LogLevel, string, Exception>>();
            var exception = new Exception();
            mock.Setup(t => t.Log(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<string>(), Array.Empty<object>())).Callback((LogLevel logLevel, Exception e, string message, object[] unused) => messageList.Add(Tuple.Create(logLevel, message, e)));
            var communicatorLogger = new NLogCommunicatorLogger(mock.Object, LogLevel.Info, LogLevel.Warn);
            communicatorLogger.Log("Hello world", exception);
            Assert.That(messageList, Has.Count.EqualTo(1));

            var record = messageList.First();
            Assert.That(record.Item2, Is.EqualTo("Hello world"));
            Assert.That(record.Item1, Is.EqualTo(LogLevel.Warn));
            Assert.That(record.Item3, Is.SameAs(exception));
        }

        [Test]
        public void TestLogExceptionIsAttachedToTheLogEvent()
        {
            // the mock above only checks which method is called; with a real NLog logger the exception must reach the log event
            var target = new MemoryTarget("memory") { Layout = "${level}: ${message} | exception=${exception:format=Type,Message}" };
            var configuration = new LoggingConfiguration();
            configuration.AddRule(LogLevel.Trace, LogLevel.Fatal, target);
            using var factory = new LogFactory();
            factory.Configuration = configuration;
            var communicatorLogger = new NLogCommunicatorLogger(factory.GetLogger("test"), LogLevel.Info, LogLevel.Warn);

            communicatorLogger.Log("Hello {world}", new InvalidOperationException("boom"));

            Assert.That(target.Logs, Has.Count.EqualTo(1));
            Assert.That(target.Logs[0], Is.EqualTo("Warn: Hello {world} | exception=System.InvalidOperationException boom"));
        }
    }
}
