using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.Core.Contracts
{
    public interface ICadService
    {
        Task<int> CreateAsync(CadModel model);

        Task CreateRangeAsync(params CadModel[] models);
        
        Task EditAsync(CadModel entity);
        
        Task DeleteAsync(int id);

        Task<bool> ExistsByIdAsync(int id);

        Task<CadModel> GetByIdAsync(int id);

        Task<CadQueryModel> GetAllAsync(CadQueryModel query);
    }
}
