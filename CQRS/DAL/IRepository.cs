namespace CQRS.DAL
{
    public interface IRepository
    {
        T[] Get<T>();
        void Set<T>(T[] items);
    }
}