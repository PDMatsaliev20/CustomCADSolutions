using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.Core.Contracts
{
    public interface ICadService
    {
        Task<int> CreateAsync(CadModel model);

        Task CreateRangeAsync(params CadModel[] models);
        
        Task EditAsync(CadModel entity);
        
        Task DeleteAsync(int id);

        Task DeleteRangeAsync(params int[] ids);
        
        Task<CadModel> GetByIdAsync(int id);

        Task<IEnumerable<CadModel>> GetAllAsync();
    }
}
