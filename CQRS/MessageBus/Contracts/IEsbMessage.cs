using System;

namespace CQRS
{
    public interface IEsbMessage
    {
        string MessageBody { get; }
        string QueueName { get; }
        TimeSpan ReceiveTimeout { get; }
    }
}