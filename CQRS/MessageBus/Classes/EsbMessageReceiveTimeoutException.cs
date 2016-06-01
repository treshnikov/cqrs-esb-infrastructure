using System;
using System.Runtime.Serialization;

namespace CQRS
{
    public class EsbMessageReceiveTimeoutException : Exception
    {
        public EsbMessageReceiveTimeoutException()
        {
        }

        public EsbMessageReceiveTimeoutException(string message) : base(message)
        {
        }

        public EsbMessageReceiveTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EsbMessageReceiveTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}