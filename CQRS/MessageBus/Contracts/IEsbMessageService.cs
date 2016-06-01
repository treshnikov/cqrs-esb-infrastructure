using System.Threading.Tasks;

namespace CQRS
{
    public interface IEsbMessageService
    {
        Task<IEsbMessageResult> SendAndGetResult(IEsbMessage query);
        Task Send(IEsbMessage command);
    }
}