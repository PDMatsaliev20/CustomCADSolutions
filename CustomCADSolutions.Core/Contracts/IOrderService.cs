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
        
        Task CreateRangeAsync(params OrderModel[] models);

        Task EditAsync(OrderModel entity);

        Task FinishOrderAsync(int id, CadModel model);

        Task EditRangeAsync(params OrderModel[] orders);

        Task DeleteAsync(int id);
    }
}
