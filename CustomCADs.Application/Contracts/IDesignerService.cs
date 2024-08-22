using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Enums;

namespace CustomCADs.Application.Contracts
{
    public interface IDesignerService
    {
        /// <summary>
        ///     Queries Cads from database.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="search"></param>
        /// <param name="pagination"></param>
        /// <returns>A Task object that represents the Result.</returns>
        Task<CadResult> GetCadsAsync(SearchModel search, PaginationModel pagination);

        /// <summary>
        ///     Overwrites Cad's Status property with the given.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        Task EditCadStatusAsync(int id, CadStatus status);

        /// <summary>
        ///     Queries Orders from database.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="designerId"></param>
        /// <param name="search"></param>
        /// <param name="pagination"></param>
        /// <returns>A Task object that represents the Result.</returns>
        Task<OrderResult> GetOrdersAsync(string status, string? designerId, SearchModel search, PaginationModel pagination);
        
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
        Task CancelAsync(int id, string designerId);

        /// <summary>
        ///     Sets the Order's Status to Finished and CadId from null to the given cadId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cadId"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        Task CompleteAsync(int id, int cadId, string designerId);
    }
}
