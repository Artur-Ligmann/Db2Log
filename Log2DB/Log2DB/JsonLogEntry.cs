namespace Log2DB
{
    public class JsonLogEntry
    {
        public string Id { get; set; }
        public LogEntryState State { get; set; }
        public string Type { get; set; }
        public string Host { get; set; }
        public long TimeStamp { get; set; }
    }
}

   
