using System.Threading.Tasks;

namespace CQRS
{
    public interface IEsbMessageService
    {
        IEsbMessageResult SendAndGetResult(IEsbMessage query);
        void Send(IEsbMessage command);
    }
}