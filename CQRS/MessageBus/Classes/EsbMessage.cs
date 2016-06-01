using System;

namespace CQRS
{
    public class EsbMessage : IEsbMessage
    {
        public EsbMessageBody MessageBody { get; }
        public string QueueName { get; }
        public TimeSpan ReceiveTimeout { get; }

        public EsbMessage(string queueName, string header, string messageBody, TimeSpan receiveTimeout)
        {
            MessageBody =  new EsbMessageBody(header, messageBody);
            QueueName = queueName;
            ReceiveTimeout = receiveTimeout;
        }

        public EsbMessage(string queueName, string header, string messageBody )
        {
            MessageBody = new EsbMessageBody(header, messageBody);
            QueueName = queueName;
            ReceiveTimeout = TimeSpan.FromSeconds(30);
        }

    }
}