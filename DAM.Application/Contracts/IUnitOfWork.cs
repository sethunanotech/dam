namespace DAM.Application.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task CompleteAsync();
    }
}
