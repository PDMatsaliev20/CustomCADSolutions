using CustomCADs.Application.Models.Roles;

namespace CustomCADs.Application.Contracts
{
    public interface IRoleService
    {
        /// <summary>
        ///     Pulls all Roles from the database
        /// </summary>
        /// <returns>The Roles</returns>
        IEnumerable<RoleModel> GetAll(string? name = null, string? description = null, string sorting = "");

        /// <summary>
        ///     Pulls all Role Names from the database
        /// </summary>
        /// <returns>The Role Names</returns>
        string[] GetAllNames();

        /// <summary>
        ///     Searches for a Role by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the Role</returns>
        /// <exception cref="KeyNotFoundException">if no Role with the given id exists.</exception>
        Task<RoleModel> GetByIdAsync(string id);
        
        /// <summary>
        ///     Searches for a Role by the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A Task object that represents the Role</returns>
        /// <exception cref="KeyNotFoundException">if no Role with the given name exists.</exception>
        Task<RoleModel> GetByNameAsync(string name);

        /// <summary>
        ///     Checks whether an Role exists by the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException">if no Role with the given id exists.</exception>
        Task<bool> ExistsByIdAsync(string id);

        /// <summary>
        ///     Checks whether an Role exists by the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="KeyNotFoundException">if no Role with the given name exists.</exception>
        Task<bool> ExistsByNameAsync(string name);

        /// <summary>
        ///     Creates the specified Role in the Roles table
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A Task object that represents the id of the created Role</returns>
        Task<string> CreateAsync(RoleModel model);

        /// <summary>
        ///     Overwrites the Role's current properties with the new ones.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        /// <exception cref="KeyNotFoundException">if no Role with the given name exists.</exception>
        Task EditAsync(string name, RoleModel model);

        /// <summary>
        ///     Deletes the Role with the given id.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the deletion.</returns>
        /// <exception cref="KeyNotFoundException">if no Role with the given id exists.</exception>
        Task DeleteAsync(string name);
    }
}
