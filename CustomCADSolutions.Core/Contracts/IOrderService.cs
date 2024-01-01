using CustomCADSolutions.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Core.Contracts
{
    public interface IOrderService
    {
        Task CreateAsync(OrderModel entity);

        Task EditAsync(OrderModel entity);

        Task DeleteAsync(int cadId, int buyerId);

        Task<OrderModel> GetByIdAsync(int cadId, int buyerId);

        Task<IEnumerable<OrderModel>> GetAll();
        Task<IEnumerable<CADModel>> GetCADsAsync();
    }
}
