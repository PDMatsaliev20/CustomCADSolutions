using CustomCADSolutions.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Core.Contracts
{
    public interface ICADService
    {
        Task CreateAsync(CADModel entity);
        
        Task EditAsync(CADModel entity);
        
        Task DeleteAsync(int id);
        
        Task<CADModel> GetByIdAsync(int id);

        Task<IEnumerable<CADModel>> GetAllAsync();
    }
}
