using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
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

        Task DeleteAsync(int id);

        Task<OrderModel> GetByIdAsync(int id);

        Task<IEnumerable<OrderModel>> GetAllAsync();
        
        IEnumerable<UserModel> GetAllUsers();
    }
}
