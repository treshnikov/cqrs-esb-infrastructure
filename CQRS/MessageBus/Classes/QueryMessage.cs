using System;

namespace CQRS
{
    public class QueryMessage : IQueryMessage
    {
        public string MessageBody { get; }
        public string QueueName { get; }
        public TimeSpan ReceiveTimeout { get; }

        public QueryMessage(string messageBody, string queueName, TimeSpan receiveTimeout)
        {
            MessageBody = messageBody;
            QueueName = queueName;
            ReceiveTimeout = receiveTimeout;
        }

        public QueryMessage(string messageBody, string queueName)
        {
            MessageBody = messageBody;
            QueueName = queueName;
            ReceiveTimeout = TimeSpan.FromSeconds(30);
        }

    }
}