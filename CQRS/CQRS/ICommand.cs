namespace CQRS
{
    public interface ICommand
    {
        string ServiceName { get; }    
    }
}