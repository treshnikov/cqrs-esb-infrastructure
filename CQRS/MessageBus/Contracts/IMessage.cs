namespace CQRS
{
    public interface IMessage
    {
        string MessageBody { get; }
        string QueueName { get; }
    }
}