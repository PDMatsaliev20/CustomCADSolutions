using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.Core.Contracts
{
    public interface ICategoryService
    {
        /// <summary>
        ///     Pulls all Categories from the database
        /// </summary>
        /// <returns>A Task object that represents the Categories</returns>
        Task<IEnumerable<Category>> GetAllAsync();

        /// <summary>
        ///     Pulls all Category names from the database
        /// </summary>
        /// <returns>A Task object that represents the Category names</returns>
        Task<IEnumerable<string>> GetAllNamesAsync();

        /// <summary>
        ///     Searches for a Category by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the Category</returns>
        /// <exception cref="KeyNotFoundException">if no Category with the given id exists.</exception>
        Task<Category> GetByIdAsync(int id);

        /// <summary>
        ///     Creates the specified Category in the Categories table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>A Task object that represents the id of the created Category</returns>
        Task<int> CreateAsync(Category entity);

        /// <summary>
        ///     Overwrites the Category's current properties with the new ones.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        /// <exception cref="KeyNotFoundException">if no Category with the given id exists.</exception>
        Task EditAsync(Category entity);

        /// <summary>
        ///     Deletes the Category with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the deletion.</returns>
        /// <exception cref="KeyNotFoundException">if no Category with the given id exists.</exception>
        Task DeleteAsync(int id);
    }
}
