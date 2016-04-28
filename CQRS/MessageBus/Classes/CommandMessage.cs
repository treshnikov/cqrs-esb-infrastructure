namespace CQRS
{
    public class CommandMessage : ICommandMessage
    {
        public string Args { get; }
        public string QueueName { get; }

        public CommandMessage(string args, string queueName)
        {
            Args = args;
            QueueName = queueName;
        }
    }
}