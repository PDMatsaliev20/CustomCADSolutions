using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Entities.Enums;
using System.Linq.Expressions;

namespace CustomCADs.Application.Contracts
{
    public interface ICadService
    {
        /// <summary>
        ///     Pulls all Cads from the Cads table in correspondence to the query.
        /// </summary>
        /// <param name="query">Specify Creator name, sorting, count, etc.</param>
        /// <returns>A Task object that represents the specified Cads.</returns>
        Task<CadResult> GetAllAsync(CadQuery query, SearchModel search, PaginationModel pagination, Expression<Func<Cad, bool>>? customFilter = null);

        /// <summary>
        ///     Searches for a Cad by the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the Cad as a Service model.</returns>
        /// <exception cref="KeyNotFoundException">if no Cad with the given id exists.</exception>
        Task<CadModel> GetByIdAsync(int id);

        /// <summary>
        ///     Checks whether a Cad exists by the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the result as a boolean.</returns>
        Task<bool> ExistsByIdAsync(int id);

        /// <summary>
        ///     Counts the Cads filtered by the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>An integer holding the result.</returns>
        int Count(Func<CadModel, bool> predicate);

        /// <summary>
        ///     Sets the Cad and the Image's path in the wwwroot folder.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cadPath"></param>
        /// <param name="imagePath"></param>
        /// <param name="otherFilesPaths"></param>
        /// <exception cref="KeyNotFoundException">if no Cad with the given id exists.</exception>
        Task SetPathsAsync(int id, string cadPath, string imagePath);

        /// <summary>
        ///     Creates the specified Cad in the Cads table.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A Task object that represents the id of the created Cad</returns>
        Task<int> CreateAsync(CadModel model);

        /// <summary>
        ///     Overwrites all of the Cad's properties with the new ones.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        /// <exception cref="KeyNotFoundException">if no Cad with the given id exists.</exception>
        Task EditAsync(int id, CadModel model);

        /// <summary>
        ///     Reverts all related Orders to Pending, sets their CadId to null and and deletes the Cad with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the deletion.</returns>
        /// <exception cref="KeyNotFoundException">if no Category with the given id exists.</exception>
        Task DeleteAsync(int id);
    }
}
