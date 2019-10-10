namespace Log2DB
{
    public class DBLogEntry
    {
        public string Id { get; set; }
        public long Duration { get; set; }
        public string Type { get; set; }
        public string Host { get; set; }
        public bool Alert { get; set; }
    }
}

   
