using System;

namespace CQRS
{
    public interface IMessageResult
    {
        bool IsError { get; }
        string ErrorText { get; }
        string Body { get; }
    }
}