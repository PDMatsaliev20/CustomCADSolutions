using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.Core.Contracts
{
    public interface ICADService
    {
        Task CreateAsync(params CadModel[] models);
        
        Task EditAsync(CadModel entity);
        
        Task DeleteAsync(int id);
        
        Task<CadModel> GetByIdAsync(int id);

        Task<IEnumerable<CadModel>> GetAllAsync();
     
        Task ImportCads(bool shouldDropDatabase);
    }
}
