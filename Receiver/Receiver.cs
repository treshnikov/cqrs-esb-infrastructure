using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CQRS
{
    public class Receiver
    {
        public Receiver(string queueName)
        {
            Receive(queueName);
        }

        private static void Receive(string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //channel.ExchangeDeclare(exchange: "logs", type: "direct");
                //var queueName = channel.QueueDeclare().QueueName;

                channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var rk = ea.RoutingKey;

                    var message = JsonConvert.DeserializeObject<EsbMessageBody>(Encoding.UTF8.GetString(body));
                    Console.WriteLine(" [x] Received {0} routing key:  {1} reply to: {2} corrId: {3}", message.Body, rk, ea.BasicProperties.ReplyTo, ea.BasicProperties.CorrelationId);
                    Thread.Sleep(3000);

                    

                    if (ea.BasicProperties.ReplyTo != null)
                    {
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = ea.BasicProperties.CorrelationId;
                        channel.BasicPublish("",
                            ea.BasicProperties.ReplyTo,
                            replyProps,
                            Encoding.UTF8.GetBytes("received!!!"));

                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                };

                channel.BasicConsume(
                    queue: queueName,
                    noAck: false,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();

            }
        }

    }

}