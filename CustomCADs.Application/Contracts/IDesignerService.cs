using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;

namespace CustomCADs.Application.Contracts;

public interface IDesignerService
{
    /// <summary>
    ///     Queries Cads from database.
    /// </summary>
    /// <param name="category"></param>
    /// <param name="name"></param>
    /// <param name="creator"></param>
    /// <param name="sorting"></param>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <returns>A Task object that represents the Result.</returns>
    CadResult GetCadsAsync(string? category = null, string? name = null, string? creator = null, string sorting = "", int page = 1, int limit = 20);

    /// <summary>
    ///     Gets the Cad with the id, as well as the previous and next one (if any).
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="CadNotFoundException"></exception>
    (int? PrevId, CadModel Current, int? NextId) GetNextCurrentAndPreviousById(int id);


    /// <summary>
    ///     Overwrites Cad's Status property with the given.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
    /// <exception cref="CadNotFoundException"></exception>
    Task EditCadStatusAsync(int id, CadStatus status);

    /// <summary>
    ///     Queries Orders from database.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="designerId"></param>
    /// <param name="search"></param>
    /// <param name="pagination"></param>
    /// <returns>A Task object that represents the Result.</returns>
    OrderResult GetOrders(string? status = "", int? id = null, string? designerId = null, string? category = null, string? name = null, string? buyer = null, string sorting = "", int page = 1, int limit = 20);

    /// <summary>
    ///     Sets the Order's Status to Begun and DesignerId from null to the given designerId.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="designerId"></param>
    /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
    /// <exception cref="OrderNotFoundException"></exception>
    Task BeginAsync(int id, string designerId);

    /// <summary>
    ///     Sets the Order's Status to Reported.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="OrderNotFoundException"></exception>
    Task ReportAsync(int id);

    /// <summary>
    ///     Sets the Order's Status to Pending and DesignerId from the given designerId to null.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
    /// <exception cref="OrderNotFoundException"></exception>
    Task CancelAsync(int id, string designerId);

    /// <summary>
    ///     Sets the Order's Status to Finished and CadId from null to the given cadId.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cadId"></param>
    /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
    /// <exception cref="OrderNotFoundException"></exception>
    Task CompleteAsync(int id, int cadId, string designerId);
}
