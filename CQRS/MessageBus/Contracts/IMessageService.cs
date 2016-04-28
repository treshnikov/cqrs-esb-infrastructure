using System.Threading.Tasks;

namespace CQRS
{
    public interface IMessageService
    {
        Task<IMessageResult> SendQueryAsync(IQueryMessage query);
        Task SendCommandAsync(ICommandMessage command);
    }

    public interface IMessageCQRSService
    {
        Task SendCommandAsync(ICommand command);
        Task<TQueryResult> SendQueryAsync<TQueryResult>(IQuery<TQueryResult> arg);
    }
}