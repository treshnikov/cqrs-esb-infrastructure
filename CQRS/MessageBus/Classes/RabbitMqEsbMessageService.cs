using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CQRS
{
    public class RabbitMqEsbMessageService : IEsbMessageService, IDisposable
    {
        private IConnection _connection;
        private IModel _channel;

        #region Ctor

        public RabbitMqEsbMessageService()
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        #endregion

        #region IEsbMessageService

        public async Task<IEsbMessageResult> SendAndGetResult(IEsbMessage query)
        {
            return await Task<IEsbMessageResult>.Factory.StartNew(() =>
            {
                try
                {
                    return DoSendQuery(query);
                }
                catch (EsbMessageReceiveTimeoutException ex)
                {
                    return new EsbMessageResult("", true, "Timeout expired " + query.ReceiveTimeout.TotalSeconds + " sec");
                }
            });
        }

        public async Task Send(IEsbMessage command)
        {
            await Task.Factory.StartNew(() => DoSendCommand(command));
        }

        #endregion

        #region Private messages

        private IEsbMessageResult DoSendQuery(IEsbMessage query)
        {
            _channel.QueueDeclare(queue: query.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var body = Encoding.UTF8.GetBytes(query.MessageBody);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ReplyTo = Guid.NewGuid().ToString("N");
            properties.CorrelationId = Guid.NewGuid().ToString("N");

            _channel.BasicPublish(
                exchange: "",
                routingKey: query.QueueName,
                basicProperties: properties,
                body: body);

            _channel.QueueDeclare(properties.ReplyTo, false, true, true, null);
            var consumer = new QueueingBasicConsumer(_channel);
            try
            {
                _channel.BasicConsume(
                queue: properties.ReplyTo,
                noAck: true,
                consumer: consumer);

            var startReceiveTime = DateTime.Now;
 
            
                while (true)
                {
                    BasicDeliverEventArgs res;
                    var answerReciwed = consumer.Queue.Dequeue(100, out res);
                    if (answerReciwed && res.BasicProperties.CorrelationId == properties.CorrelationId)
                    {
                        var answer = Encoding.UTF8.GetString(res.Body);
                        return new EsbMessageResult(answer);
                    }

                    if (DateTime.Now - startReceiveTime > query.ReceiveTimeout)
                    {
                        throw new EsbMessageReceiveTimeoutException();
                    }
                }
            }
            finally
            {
                _channel.QueueDelete(properties.ReplyTo);
                consumer.Queue.Close();

            }
            
        }

        private void DoSendCommand(IEsbMessage query)
        {
            _channel.QueueDeclare(
                queue: query.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var body = Encoding.UTF8.GetBytes(query.MessageBody);

            var properties = _channel.CreateBasicProperties();
            properties.SetPersistent(true);

            _channel.BasicPublish(
                exchange: "",
                routingKey: query.QueueName,
                basicProperties: properties,
                body: body);
        }


        #endregion

        #region IDisposable

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        #endregion

    }
}