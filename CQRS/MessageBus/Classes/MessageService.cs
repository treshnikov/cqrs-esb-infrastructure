using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CQRS
{
    public class MessageService : IMessageService, IMessageCQRSService, IDisposable
    {
        private IConnection _connection;
        private IModel _channel;

        #region Ctor

        public MessageService()
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        #endregion

        #region IMessageService

        public async Task<IMessageResult> SendQueryAsync(IQueryMessage query)
        {
            return await Task<IMessageResult>.Factory.StartNew(() =>
            {
                try
                {
                    return DoSendQuery(query);
                }
                catch (MessageReceiveTimeoutException ex)
                {
                    return new MessageResult("", true, "Timeout expired " + query.ReceiveTimeout.TotalSeconds + " sec");
                }
            });
        }

        public async Task SendCommandAsync(ICommandMessage command)
        {
            await Task.Factory.StartNew(() => DoSendCommand(command));
        }

        public void SendCommand(ICommandMessage command)
        {
            DoSendCommand(command);
        }

        #endregion

        #region IMessageCQRSService

        public async Task SendCommandAsync(ICommand command)
        {
            // получить им€ очереди в которую надо отправить данные = им€ сервиса
            // сериализовать данные команды
            // отправить команду
            var jsonCommand = JsonConvert.SerializeObject(command);
            await SendCommandAsync(new CommandMessage(jsonCommand, command.ServiceName));
        }

        public async Task<TQueryResult> SendQueryAsync<TQueryResult>(IQuery<TQueryResult> arg)
        {
            // получить им€ очереди в которую надо отправить данные = им€ сервиса
            // сериализовать данные запроса
            // отправить запрос
            // получить данные, десериализовать в объект
            var jsonQuery = JsonConvert.SerializeObject(arg);
            var res = await SendQueryAsync(new QueryMessage(jsonQuery, arg.ServiceName));

            // todo res.IsError
            return JsonConvert.DeserializeObject<TQueryResult>(res.Body);
        }

        #endregion

        #region Private messages

        private IMessageResult DoSendQuery(IQueryMessage query)
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
                    return new MessageResult(answer);
                }

                if (DateTime.Now - startReceiveTime > query.ReceiveTimeout)
                {
                    throw new MessageReceiveTimeoutException();
                }
            }
        }

        private void DoSendCommand(ICommandMessage query)
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