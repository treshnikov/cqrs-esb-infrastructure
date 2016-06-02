namespace CQRS
{
    public interface IQueryHandler<TQuery, TQueryResult> where TQuery : IQuery
    {
        TQueryResult Handle(TQuery query);
    }
}