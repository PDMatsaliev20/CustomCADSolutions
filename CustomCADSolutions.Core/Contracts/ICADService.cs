using CustomCADSolutions.Core.Models;
using System.Drawing;

namespace CustomCADSolutions.Core.Contracts
{
    public interface ICadService
    {
        Task<CadQueryModel> GetAllAsync(CadQueryModel query);

        Task<CadModel> GetByIdAsync(int id);

        Task<bool> ExistsByIdAsync(int id);

        Task ChangeColorAsync(int id, Color color);

        int Count(Func<CadModel, bool> predicate);

        Task<int> CreateAsync(CadModel model);

        Task CreateRangeAsync(params CadModel[] models);
        
        Task EditAsync(int id, CadModel entity);
        
        Task DeleteAsync(int id);
    }
}
