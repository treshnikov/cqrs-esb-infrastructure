namespace CQRS.DAL
{
    public interface IRepository
    {
        T[] Get<T>();
    }
}