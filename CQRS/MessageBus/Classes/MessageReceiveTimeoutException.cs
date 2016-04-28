using System;
using System.Runtime.Serialization;

namespace CQRS
{
    public class MessageReceiveTimeoutException : Exception
    {
        public MessageReceiveTimeoutException()
        {
        }

        public MessageReceiveTimeoutException(string message) : base(message)
        {
        }

        public MessageReceiveTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MessageReceiveTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}