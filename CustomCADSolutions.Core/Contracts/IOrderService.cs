using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.Core.Contracts
{
    public interface IOrderService
    {
        Task<(string, int)> CreateAsync(OrderModel entity);
        
        Task CreateRangeAsync(params OrderModel[] models);

        Task EditAsync(OrderModel entity);
        
        Task EditRangeAsync(params OrderModel[] orders);

        Task DeleteAsync(int cadId, string buyerId);

        Task<OrderModel> GetByIdAsync(int cadId, string buyerId);

        Task<IEnumerable<OrderModel>> GetAllAsync();
        Task<bool> ExistsByIdAsync(int cadId, string buyerId);
    }
}
