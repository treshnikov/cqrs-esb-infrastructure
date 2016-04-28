namespace CQRS
{
    public class MessageResult : IMessageResult
    {
        public bool IsError { get; }
        public string ErrorText { get; }
        public string Body { get; }

        public MessageResult(string body, bool isError = false, string errorText = "")
        {
            Body = body;
            IsError = isError;
            ErrorText = errorText;
        }

    }
}