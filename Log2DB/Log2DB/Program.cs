using System;

namespace Log2DB
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
                    throw new InvalidOperationException("No path specified.");

                var path = args[0];

                var input = FileLogService.LoadFromFile(path + @"\logfile.txt");
                var output = new LogWriterService("output.db", "LogEntries");
                var service = new LogParsingService(input, output, 4);

                service.Process();
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
