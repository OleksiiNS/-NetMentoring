using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ProcessingServer
{
    class Program
    {
        private const string ExchangeName = "serviceExchange";
        private const string QueueName = "processingQueue";
        private const string RabbitMqUrl = "amqp://guest:guest@localhost:5672";
        private const string RoutingKey = "data.process";
        private const string ReceivedFileDirectory = @"c:\test\received\";
        private static IConnection? _connection;
        private static IModel? _channel;

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(RabbitMqUrl)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(QueueName, true);
            _channel.QueueBind(QueueName, ExchangeName, RoutingKey);
            Console.WriteLine("Processor Service is started.");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var headers = eventArgs.BasicProperties.Headers;
                var filename =  headers != null && headers.ContainsKey("filename") ? Encoding.UTF8.GetString((byte[])headers["filename"]) : Guid.NewGuid().ToString();
                var fullname = $"{ReceivedFileDirectory}{filename}";
                var seq =  headers != null && headers.ContainsKey("seq") ? (int)headers["seq"] : 0;
                if (seq == 0)
                {
                    using var stream = File.Create(fullname);
                    stream.Write(body, 0, body.Length);
                    Console.WriteLine($"  File {fullname} part {seq} is received");
                    stream.Close();
                }
                else
                {
                    using var stream = File.Open(fullname, FileMode.Append);
                    stream.Write(body, 0, body.Length);
                    Console.WriteLine($"  File {fullname} part {seq} is received");
                    stream.Close();
                }

            };

            _channel.BasicConsume(QueueName, true, consumer);

            Console.ReadLine();
            Console.WriteLine("Processor Service is finished.");

            _channel.Close();
            _connection.Close();
        }
    }
}
