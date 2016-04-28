namespace CQRS
{
    public interface IMessage
    {
        string Args { get; }
        string QueueName { get; }
    }
}