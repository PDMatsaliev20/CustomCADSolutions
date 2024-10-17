namespace CustomCADs.Domain.Shared;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
