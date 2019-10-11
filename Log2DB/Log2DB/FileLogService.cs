using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Log2DB
{
    public class FileLogService : ILogRepository
    {
        readonly Dictionary<string, JsonLogEntry> _entriesStart;
        readonly Dictionary<string, JsonLogEntry> _entriesStop;

        public FileLogService(Dictionary<string, JsonLogEntry> entriesStart, Dictionary<string, JsonLogEntry> entriesStop)
        {
            _entriesStart = entriesStart;
            _entriesStop = entriesStop;
        }

        public static ILogRepository LoadFromFile(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            var entriesStart = new Dictionary<string, JsonLogEntry>();
            var entriesStop = new Dictionary<string, JsonLogEntry>();

            using (StreamReader file = File.OpenText(filePath))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var entry = JsonConvert.DeserializeObject<JsonLogEntry>(line);

                    switch (entry.State)
                    {
                        case LogEntryState.STARTED:
                            {
                                entriesStart[entry.Id] = entry;
                                break;
                            }
                        case LogEntryState.FINISHED:
                            {
                                entriesStop[entry.Id] = entry;
                                break;
                            }
                    }
                }
            }
            return new FileLogService(entriesStart, entriesStop);
        }

        public Dictionary<string, JsonLogEntry> GetStartedEntries()
        {
            return _entriesStart;
        }

        public Dictionary<string, JsonLogEntry> GetStoppedEntries()
        {
            return _entriesStop;
        }
    }

    
}

   
