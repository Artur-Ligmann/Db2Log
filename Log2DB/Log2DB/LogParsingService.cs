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
                    var timeDifferenece = entriesStop[entry.Key].TimeStamp - entry.Value.TimeStamp;

                    var value = entry.Value;
                    var dbEntry = new DBLogEntry()
                    {
                        Duration = timeDifferenece,
                        Alert = timeDifferenece > _alertThreshold,
                        Id = entry.Key,
                        Host = value.Host,
                        Type = !String.IsNullOrEmpty(value.Type) ? value.Type.ToString() : String.Empty
                    };

                    _dbWriteRepository.InsertLogEntry(dbEntry);
                }
            }
        }
  
}

   
