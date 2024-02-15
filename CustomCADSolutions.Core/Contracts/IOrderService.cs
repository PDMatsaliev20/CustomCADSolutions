using CustomCADSolutions.Core.Models;

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
