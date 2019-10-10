using LiteDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Log2DB.Tests
{
    [TestClass]
    public  class Log2DBIntegratedTestFixture
    {
        [TestMethod]
        public void TestLog2DB()
        {
            // Arrange
            var testDbPath = @"MyData.db";
            File.Delete(testDbPath);

            var input = FileLogService.LoadFromFile(@"testlogfile.txt");
            var output = new LogWriterService(testDbPath, "LogEntries");
            var service = new LogParsingService(input, output, 4);

            // Act
            service.Process();

            // Assert
            var result = output.GetLogEntries();
            Assert.IsTrue(result.Count == 3);

        }
    }
}

   
