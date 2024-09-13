using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;

namespace CustomCADs.Application.Contracts
{
    public interface IOrderService
    {
        /// <summary>
        ///     Pulls all Orders from the Orders table.
        /// </summary>
        /// <returns>A Task object that represents the Orders.</returns>
        OrderResult GetAll(string? buyer = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", int page = 1, int limit = 20, Func<OrderModel, bool>? customFilter = null);

        /// <summary>
        ///     Searches for an Order by the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task that represents the Order as a service model.</returns>
        /// <exception cref="KeyNotFoundException">if no Order with the given id exists.</exception>
        Task<OrderModel> GetByIdAsync(int id);
        
        /// <summary>
        ///     Searches for the Order's Cad by the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task that represents the Order as a service model.</returns>
        /// <exception cref="KeyNotFoundException">if no Order with the given id exists.</exception>
        Task<CadModel> GetCadAsync(int id);

        ///<summary>
        ///     Checks whether an Order exists by the given id.
        ///</summary>
        /// <exception cref="KeyNotFoundException">if no Order with the given id exists.</exception>
        Task<bool> ExistsByIdAsync(int id);
        
        /// <summary>
        ///     Counts the Orders filtered by the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>An integer holding the result.</returns>
        Task<int> CountAsync(Func<OrderModel, bool> predicate);

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

        /// <summary>
        ///     Checks if an Order has a CadId property.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Whether it does or not.</returns>
        /// <exception cref="KeyNotFoundException">if no Order with the given id exists.</exception>/// <summary>
        Task<bool> HasCadAsync(int id);

        /// <summary>
        ///     Checks if an Order's Buyer is the given.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns>Whether it is or not.</returns>
        /// <exception cref="KeyNotFoundException">if no Order with the given id exists.</exception>
        Task<bool> CheckOwnership(int id, string username);
    }
}
