using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.Core.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetAllAsync();

        Task<OrderModel> GetByIdAsync(int id);

        Task<bool> ExistsByIdAsync(int id);

        Task<int> CreateAsync(OrderModel entity);
        
        Task EditAsync(int id, OrderModel entity);

        Task FinishOrderAsync(int id, OrderModel model);

        Task DeleteAsync(int id);
    }
}
