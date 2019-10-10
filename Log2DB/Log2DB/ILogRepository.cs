using System.Collections.Generic;

namespace Log2DB
{
    public interface ILogRepository
    {
        Dictionary<string, JsonLogEntry> GetStartedEntries();
        Dictionary<string, JsonLogEntry> GetStoppedEntries();
    }

    
}

   
