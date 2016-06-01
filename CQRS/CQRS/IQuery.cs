namespace CQRS
{
    public interface IAbstractQuery
    {
        string ServiceName { get; }
    }

    public interface IQuery<TQueryResult> : IAbstractQuery
    {
    }
}