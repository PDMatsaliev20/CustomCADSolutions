using CustomCADs.Core.Models;

namespace CustomCADs.Core.Contracts
{
    public interface ICategoryService
    {
        /// <summary>
        ///     Pulls all Categories from the database
        /// </summary>
        /// <returns>A Task object that represents the Categories</returns>
        Task<IEnumerable<CategoryModel>> GetAllAsync();

        /// <summary>
        ///     Searches for a Category by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the Category</returns>
        /// <exception cref="KeyNotFoundException">if no Category with the given id exists.</exception>
        Task<CategoryModel> GetByIdAsync(int id);

        /// <summary>
        ///     Creates the specified Category in the Categories table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>A Task object that represents the id of the created Category</returns>
        Task<int> CreateAsync(CategoryModel entity);

        /// <summary>
        ///     Overwrites the Category's current properties with the new ones.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        /// <exception cref="KeyNotFoundException">if no Category with the given id exists.</exception>
        Task EditAsync(int id, CategoryModel model);

        /// <summary>
        ///     Deletes the Category with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the deletion.</returns>
        /// <exception cref="KeyNotFoundException">if no Category with the given id exists.</exception>
        Task DeleteAsync(int id);
    }
}
