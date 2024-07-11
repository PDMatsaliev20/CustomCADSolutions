using CustomCADs.Core.Models;

namespace CustomCADs.Core.Contracts
{
    public interface IOrderService
    {
        /// <summary>
        ///     Pulls all Orders from the Orders table.
        /// </summary>
        /// <returns>A Task object that represents the Orders.</returns>
        Task<IEnumerable<OrderModel>> GetAllAsync();

        /// <summary>
        ///     Searches for an Order by the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task that represents the Order as a service model.</returns>
        /// <exception cref="KeyNotFoundException">if no Order with the given id exists.</exception>
        Task<OrderModel> GetByIdAsync(int id);

        ///<summary>
        ///     Checks whether an Order exists by the given id.
        ///</summary>
        /// <exception cref="KeyNotFoundException">if no Order with the given id exists.</exception>
        Task<bool> ExistsByIdAsync(int id);

        /// <summary>
        ///     Creates the specified Order in the Orders table.
        /// </summary>
        /// <param name="model"></param>
        /// /// <returns>A Task object that represents the id of the created Order.</returns>
        Task<int> CreateAsync(OrderModel model);

        /// <summary>
        ///     Overwrites the Order's current properties with the new ones.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        /// <exception cref="KeyNotFoundException">if no Order with the given id exists.</exception>
        Task EditAsync(int id, OrderModel entity);

        /// <summary>
        ///     Deletes the Order with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the deletion.</returns>
        /// <exception cref="KeyNotFoundException">if no Order with the given id exists.</exception>
        Task DeleteAsync(int id);
        IList<string> ValidateEntity(OrderModel model);
    }
}
