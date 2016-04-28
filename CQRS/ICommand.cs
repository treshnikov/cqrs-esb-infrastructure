namespace CQRS
{
    public interface ICommand<in TContext>
    {
        void Execute(TContext context);
    }

    public interface IQuery<T>
    {
        T Execute();
    }

}