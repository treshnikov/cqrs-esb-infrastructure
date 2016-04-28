using System;

namespace CQRS
{
    public interface IQueryMessage : IMessage
    {
        TimeSpan ReceiveTimeout { get; }
    }
}