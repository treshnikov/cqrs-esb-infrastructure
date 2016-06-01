using System;

namespace CQRS
{
    public class EsbMessage : IEsbMessage
    {
        public string MessageBody { get; }
        public string QueueName { get; }
        public TimeSpan ReceiveTimeout { get; }

        public EsbMessage(string messageBody, string queueName, TimeSpan receiveTimeout)
        {
            MessageBody = messageBody;
            QueueName = queueName;
            ReceiveTimeout = receiveTimeout;
        }

        public EsbMessage(string messageBody, string queueName)
        {
            MessageBody = messageBody;
            QueueName = queueName;
            ReceiveTimeout = TimeSpan.FromSeconds(30);
        }

    }
}