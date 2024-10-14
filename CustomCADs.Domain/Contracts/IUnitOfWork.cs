namespace CustomCADs.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();   
    }
}
