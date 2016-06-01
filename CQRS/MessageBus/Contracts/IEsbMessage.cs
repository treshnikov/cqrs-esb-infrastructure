using System;

namespace CQRS
{
    public interface IEsbMessage
    {
        EsbMessageBody MessageBody { get; }
        string QueueName { get; }
        TimeSpan ReceiveTimeout { get; }
    }

    public class EsbMessageBody
    {
        public string Header { get; set; }
        public string Body { get; set; }

        public EsbMessageBody(string header, string body)
        {
            Header = header;
            Body = body;
        }
    }
}