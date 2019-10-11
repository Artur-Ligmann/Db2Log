using System;

namespace Log2DB
{
        public class LogParsingService
        {
            private readonly ILogRepository _inputRepository;
            private readonly IDBLogWriter _dbWriteRepository;
            private readonly int _alertThreshold;

           public LogParsingService(ILogRepository inputRepository, IDBLogWriter dbWriteRepository, int alertThresholdMS = 4)
            {
                _inputRepository = inputRepository;
                _dbWriteRepository = dbWriteRepository;
                _alertThreshold = alertThresholdMS;
            }

            public void Process()
            {
                var entriesStart = _inputRepository.GetStartedEntries();
                var entriesStop = _inputRepository.GetStoppedEntries();

                foreach (var entry in entriesStart)
                {
                    if(!entriesStop.ContainsKey(entry.Key))
                    {
                        continue;
                    }


                    var timeDifferenece = entriesStop[entry.Key].TimeStamp - entry.Value.TimeStamp;

                    var value = entry.Value;
                    var host = value.Host ?? entriesStop[entry.Key].Host;
                    var type = value.Type ?? entriesStop[entry.Key].Type;

                    var dbEntry = new DBLogEntry()
                    {
                        Duration = timeDifferenece,
                        Alert = timeDifferenece > _alertThreshold,
                        Id = entry.Key,
                        Host = host,
                        Type = !String.IsNullOrEmpty(type) ? type.ToString() : String.Empty
                    };

                    _dbWriteRepository.InsertLogEntry(dbEntry);
                }
            }
        }
  
}

   
