namespace CQRS
{
    public class EsbMessageResult : IEsbMessageResult
    {
        public bool IsError { get; }
        public string ErrorText { get; }
        public string Body { get; }

        public EsbMessageResult(string body, bool isError = false, string errorText = "")
        {
            Body = body;
            IsError = isError;
            ErrorText = errorText;
        }

    }
}