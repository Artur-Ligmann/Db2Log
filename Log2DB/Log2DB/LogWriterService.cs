using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace Log2DB
{
    public class LogWriterService : IDBLogWriter
    {
        readonly LiteDatabase _db;
        readonly string _tableName;

        public LogWriterService(string dbFile, string tableName)
        {
            _db = new LiteDatabase(dbFile);
            _tableName = tableName;
        }

        public void InsertLogEntry(DBLogEntry entry)
        {
            var dbLogEntries = _db.GetCollection<DBLogEntry>(_tableName);
            dbLogEntries.Insert(entry);
        }

        public IList<DBLogEntry> GetLogEntries()
        {
            var dbLogEntries = _db.GetCollection<DBLogEntry>(_tableName);
            return dbLogEntries.FindAll().ToList();
        }
    }
}

   
