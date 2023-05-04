using System.Text;
using RabbitMQ.Client;


namespace DataCaptureService
{
    class Program
    {
        private const string ExchangeName = "serviceExchange";
        private const string RabbitMqUrl = "amqp://guest:guest@localhost:5672";
        private const string RoutingKey = "data.process";
        private const int MaxChunkSizeInBytes = 50000000;
        private static string _inFileDirectory = @"c:\test\in\";
        private static FileSystemWatcher? _fileWatcher;
        private static IConnection? _connection;
        private static IModel? _channel;
        private static IBasicProperties? _props;

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                _inFileDirectory = args[0];
            }

            var factory = new ConnectionFactory
            {
                Uri = new Uri(RabbitMqUrl)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _props = _channel.CreateBasicProperties();
            Console.WriteLine($"Data Service is started on {_inFileDirectory}");

            _fileWatcher = new FileSystemWatcher(_inFileDirectory);
            _fileWatcher.IncludeSubdirectories = true;
            _fileWatcher.NotifyFilter = NotifyFilters.FileName;
            _fileWatcher.EnableRaisingEvents = true;
            _fileWatcher.Created += WatcherActivity;
            
            Console.ReadLine();
            Console.WriteLine("Data Service is finished");

            _channel.Close();
            _connection.Close();
        }

        private static void WatcherActivity(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(10000);
           
            var fs = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read);
            var fileBytes = new byte[fs.Length];
            _ = fs.Read(fileBytes, 0, fileBytes.Length);
            fs.Close();
            var chunks = SplitBytes(fileBytes, MaxChunkSizeInBytes);
            for (var i = 0; i < chunks.Length; i++)
            {
                _props!.Headers = new Dictionary<string, object> { { "filename", Encoding.UTF8.GetBytes(e.Name!) }, {"seq", i} };
                _channel.BasicPublish(ExchangeName, RoutingKey, _props, chunks[i].ToArray());
                Console.WriteLine($"  File {e.FullPath} part {i} is sent to Processor");
            }

            Console.WriteLine($"  File {e.FullPath} is finished sending to Processor");
            File.Delete(e.FullPath);
        }

        private static byte[][] SplitBytes(byte[] buffer, int blockSize)
        {
            var blocks = new byte[(buffer.Length + blockSize - 1) / blockSize][];

            for (int i = 0, j = 0; i < blocks.Length; i++, j += blockSize)
            {
                blocks[i] = new byte[Math.Min(blockSize, buffer.Length - j)];
                Array.Copy(buffer, j, blocks[i], 0, blocks[i].Length);
            }

            return blocks;
        }
    }
}
