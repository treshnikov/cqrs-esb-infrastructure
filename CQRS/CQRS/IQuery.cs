namespace CQRS
{
    public interface IQuery<TQueryResult>
    {
        string ServiceName { get; }
    }
}