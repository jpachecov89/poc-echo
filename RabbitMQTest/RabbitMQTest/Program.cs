using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace RabbitMQTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Test
            //TestExample();

            // Exchange direct
            ExchangeDirectTest();
        }

        private static void TestExample()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                    string message = "Hello World";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                        routingKey: "hello",
                                        basicProperties: null,
                                        body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void ExchangeDirectTest()
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);
                string value = Guid.NewGuid().ToString();
                string logMessage = string.Format("{0}: {1}", TraceEventType.Information, value);

                var message = Encoding.UTF8.GetBytes(logMessage);
                channel.BasicPublish("direct_logs", "info", null, message);

                Console.WriteLine("SEND: {0}", logMessage);
            }
            Console.ReadKey();
        }
    }
}
