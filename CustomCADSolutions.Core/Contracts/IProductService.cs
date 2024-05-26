using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.Core.Contracts
{
    public interface IProductService
    {
        /// <summary>
        ///     Pulls all Products from the Products table.
        /// </summary>
        /// <returns>A Task object that represents all Products.</returns>
        Task<IEnumerable<ProductModel>> GetAllAsync();

        /// <summary>
        ///     Searches for a Product by the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the Product as a Service model.</returns>
        /// <exception cref="KeyNotFoundException">if no Product with the given id exists.</exception>
        Task<ProductModel> GetByIdAsync(int id);

        /// <summary>
        ///     Checks whether a Product exists by the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the result as a boolean.</returns>
        Task<bool> ExistsByIdAsync(int id);

        /// <summary>
        ///     Creates the specified Product in the Products table.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A Task object that represents the id of the created Product</returns>
        Task<int> CreateAsync(ProductModel model);

        /// <summary>
        ///     Overwrites the Product's properties with the new ones.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        /// <exception cref="KeyNotFoundException">if no Cad with the given id exists.</exception>
        Task EditAsync(int id, ProductModel model);

        /// <summary>
        ///     Reverts all related Orders to Pending, sets their ProductId to null and and deletes the Product with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the deletion.</returns>
        /// <exception cref="KeyNotFoundException">if no Product with the given id exists.</exception>
        Task DeleteAsync(int id);
        IList<string> ValidateEntity(OrderModel model);
    }
}
