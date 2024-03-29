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

        Task DeleteRangeAsync(params int[] ids);

        Task<bool> ExistsByIdAsync(int id);

        Task<CadModel> GetByIdAsync(int id);

        Task<CadQueryModel> GetAllAsync(
            string? category = null,string? creatorName = null,
            string? searchName = null, string? searchCreator = null,
            CadSorting sorting = CadSorting.Newest,
            int currentPage = 1,int modelsPerPage = 1,
            bool validated = true, bool unvalidated = false);
    }
}
