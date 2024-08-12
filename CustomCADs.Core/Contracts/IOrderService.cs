using CustomCADs.Core.Models;
using CustomCADs.Core.Models.Cads;
using CustomCADs.Core.Models.Orders;
using CustomCADs.Domain.Entities;
using System.Linq.Expressions;

namespace CustomCADs.Core.Contracts
{
    public interface IOrderService
    {
        /// <summary>
        ///     Pulls all Orders from the Orders table.
        /// </summary>
        /// <returns>A Task object that represents the Orders.</returns>
        Task<OrderResult> GetAllAsync(OrderQuery query, SearchModel search, PaginationModel pagination, Expression<Func<Order, bool>>? customFilter = null);

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
        ///     Sets the Order's Status to Begun and DesignerId from null to the given designerId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="designerId"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        Task BeginAsync(int id, string designerId);

        /// <summary>
        ///     Sets the Order's Status to Reported.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ReportAsync(int id);

        /// <summary>
        ///     Sets the Order's Status to Pending and DesignerId from the given designerId to null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        Task CancelAsync(int id);
        
        /// <summary>
        ///     Sets the Order's Status to Finished and CadId from null to the given cadId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cadId"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        Task CompleteAsync(int id, int cadId);

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
