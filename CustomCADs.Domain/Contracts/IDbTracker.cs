namespace CustomCADs.Domain.Contracts
{
    public interface IDbTracker
    {
        Task<int> SaveChangesAsync();   
    }
}
