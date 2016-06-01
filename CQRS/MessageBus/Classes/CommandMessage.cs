namespace CQRS
{
    public class CommandMessage : ICommandMessage
    {
        public string MessageBody { get; }
        public string QueueName { get; }

        public CommandMessage(string messageBody, string queueName)
        {
            MessageBody = messageBody;
            QueueName = queueName;
        }
    }
}