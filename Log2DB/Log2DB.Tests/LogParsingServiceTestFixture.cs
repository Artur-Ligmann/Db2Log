using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Log2DB.UnitTests
{
    [TestClass]
    public class LogParsingServiceTestFixture

    {
        [TestMethod]
        public void Process_WhenOneOrMoreInputEntryHasNoType_ShouldNotThrowException()
        {
            var outputMock = new Mock<IDBLogWriter>();
            var inputMock = new Mock<ILogRepository>();
            Dictionary<string, JsonLogEntry> fileLogDataStart = CreateInputWithOneStartedEventWithoutType();
            Dictionary<string, JsonLogEntry> fileLogDataStop = CreateInputWithOneStoppedEventWithoutType();

            inputMock.Setup(i => i.GetStartedEntries()).Returns(fileLogDataStart);
            inputMock.Setup(i => i.GetStoppedEntries()).Returns(fileLogDataStop);


            var sut = new LogParsingService(inputMock.Object, outputMock.Object);


            sut.Process();

            Assert.IsTrue(true);
        }



        [TestMethod]
        public void Process_WhenInputHasOneEntryDurationOverThreshold_EntryInDbShouldBeAlerted()
        {
            // Arrange
            var outputMock = new Mock<IDBLogWriter>();
            var inputMock = new Mock<ILogRepository>();

            Dictionary<string, JsonLogEntry> fileLogDataStart = CreateInputWithOneStartedEventWithZeroTimeStamp();
            Dictionary<string, JsonLogEntry> fileLogDataStop = CreateInputWithOneStoppedEventWithTenTimeStamp();

            inputMock.Setup(i => i.GetStartedEntries()).Returns(fileLogDataStart);
            inputMock.Setup(i => i.GetStoppedEntries()).Returns(fileLogDataStop);


            var sut = new LogParsingService(inputMock.Object, outputMock.Object, 4);

            // Act
            sut.Process();

            // Assert
            outputMock.Verify(o => o.InsertLogEntry(It.Is<DBLogEntry>(l => l.Id == "id" && l.Alert == true)));      
       }

        private static Dictionary<string, JsonLogEntry> CreateInputWithOneStoppedEventWithoutType()
        {
            return new Dictionary<string, JsonLogEntry>()
            {
                { "id", new JsonLogEntry()
                    {
                        Id = "id",
                        State = LogEntryState.FINISHED,
                        TimeStamp = 10,
                        Host = "host"
                    }
                }

            };
        }

        private static Dictionary<string, JsonLogEntry> CreateInputWithOneStartedEventWithoutType()
        {
            return new Dictionary<string, JsonLogEntry>()
            {
                { "id", new JsonLogEntry()
                    {
                        Id = "id",
                        State = LogEntryState.STARTED,
                        TimeStamp = 0,
                        Host = "host"
                    }
                }

            };
        }

        private static Dictionary<string, JsonLogEntry> CreateInputWithOneStoppedEventWithTenTimeStamp()
        {
            return new Dictionary<string, JsonLogEntry>()
            {
                { "id", new JsonLogEntry()
                    {
                        Id = "id",
                        State = LogEntryState.FINISHED,
                        TimeStamp = 10,
                        Host = "host"
                    }
                }

            };

        }
        Dictionary<string, JsonLogEntry> CreateInputWithOneStartedEventWithZeroTimeStamp()
        {
            return new Dictionary<string, JsonLogEntry>()
            {
                { "id", new JsonLogEntry()
                    {
                        Id = "id",
                        State = LogEntryState.STARTED,
                        TimeStamp = 0,
                        Host = "host"
                    }
                }

            };
        }
    }
}
