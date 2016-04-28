using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CQRS
{
    public class Receiver
    {
        public Receiver()
        {
            Receive();
        }

        private static void Receive()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //channel.ExchangeDeclare(exchange: "logs", type: "direct");
                //var queueName = channel.QueueDeclare().QueueName;

                var queueName = "rpc";
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

                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0} routing key:  {1} reply to: {2} corrId: {3}", message, rk, ea.BasicProperties.ReplyTo, ea.BasicProperties.CorrelationId);
                    Thread.Sleep(3000);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                    if (ea.BasicProperties.ReplyTo != null)
                    {
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = ea.BasicProperties.CorrelationId;
                        channel.BasicPublish("",
                            ea.BasicProperties.ReplyTo,
                            replyProps,
                            Encoding.UTF8.GetBytes("received!!!"));
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