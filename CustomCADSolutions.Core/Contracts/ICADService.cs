using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.Core.Contracts
{
    public interface ICadService
    {
        Task CreateAsync(params CadModel[] models);
        
        Task EditAsync(CadModel entity);
        
        Task DeleteAsync(int id);

        Task DeleteRangeAsync(params int[] ids);
        
        Task<CadModel> GetByIdAsync(int id);

        Task<IEnumerable<CadModel>> GetAllAsync();

        Task UpdateCads(bool shouldResetDb = false);
    }
}
