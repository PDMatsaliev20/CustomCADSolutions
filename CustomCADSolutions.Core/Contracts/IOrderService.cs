using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
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
        
        Task CreateRangeAsync(params OrderModel[] models);

        Task EditAsync(OrderModel entity);

        Task DeleteAsync(int cadId, string buyerId);

        Task<OrderModel> GetByIdAsync(int cadId, string buyerId);

        Task<IEnumerable<OrderModel>> GetAllAsync();
    }
}
