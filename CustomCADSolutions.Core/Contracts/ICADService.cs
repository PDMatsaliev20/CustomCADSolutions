using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Core.Contracts
{
    public interface ICADService
    {
        Task CreateAsync(CadModel entity);
        
        Task EditAsync(CadModel entity);
        
        Task DeleteAsync(int id);
        
        Task<CadModel> GetByIdAsync(int id);

        Task<IEnumerable<CadModel>> GetAllAsync();
    }
}
