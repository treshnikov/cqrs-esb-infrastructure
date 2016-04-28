namespace CQRS
{
    public interface IQueryHandler<TQuery, TQueryResult>
    {
        TQueryResult Handle(TQuery query);
    }
}