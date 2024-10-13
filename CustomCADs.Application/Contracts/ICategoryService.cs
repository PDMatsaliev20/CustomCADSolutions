using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Models.Categories;

namespace CustomCADs.Application.Contracts
{
    public interface ICategoryService
    {
        /// <summary>
        ///     Pulls all Categories from the database
        /// </summary>
        /// <returns>A Task object that represents the Categories</returns>
        IEnumerable<CategoryModel> GetAll(Func<CategoryModel, bool>? customFilter = null);

        /// <summary>
        ///     Searches for a Category by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the Category</returns>
        /// <exception cref="CategoryNotFoundException">if no Category with the given id exists.</exception>
        Task<CategoryModel> GetByIdAsync(int id);

        /// <summary>
        ///     Checks if the Category with the provided id exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents whether the Category exists</returns>
        Task<bool> ExistsByIdAsync(int id);

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
        /// <exception cref="CategoryNotFoundException">if no Category with the given id exists.</exception>
        Task EditAsync(int id, CategoryModel model);

        /// <summary>
        ///     Deletes the Category with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the deletion.</returns>
        /// <exception cref="CategoryNotFoundException">if no Category with the given id exists.</exception>
        Task DeleteAsync(int id);
    }
}
