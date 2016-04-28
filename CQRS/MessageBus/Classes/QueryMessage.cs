using System;

namespace CQRS
{
    public class QueryMessage : IQueryMessage
    {
        public string Args { get; }
        public string QueueName { get; }
        public TimeSpan ReceiveTimeout { get; }

        public QueryMessage(string args, string queueName, TimeSpan receiveTimeout)
        {
            Args = args;
            QueueName = queueName;
            ReceiveTimeout = receiveTimeout;
        }

        public QueryMessage(string args, string queueName)
        {
            Args = args;
            QueueName = queueName;
            ReceiveTimeout = TimeSpan.FromSeconds(30);
        }

    }
}