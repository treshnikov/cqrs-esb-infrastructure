using System;

namespace CQRS
{
    public interface IEsbMessageResult
    {
        bool IsError { get; }
        string ErrorText { get; }
        string Body { get; }
    }
}