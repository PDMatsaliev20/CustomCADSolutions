using CustomCADs.Application.Models.Users;

namespace CustomCADs.Application.Contracts
{
    public interface IUserService
    {
        /// <summary>
        ///     Pulls specified Users from the database
        /// </summary>
        /// <returns>A Task object that represents the Users</returns>
        UserResult GetAll(bool? hasRT = null, string? username = null, string? email = null, string? firstName = null, string? lastName = null, DateTime? rtEndDateBefore = null, DateTime? rtEndDateAfter = null, string sorting = "", int page = 1, int limit = 20, Func<UserModel, bool>? customFilter = null);

        /// <summary>
        ///     Searches for a User by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the User</returns>
        /// <exception cref="KeyNotFoundException">if no User with the given id exists.</exception>
        Task<UserModel> GetByIdAsync(string id);
        
        /// <summary>
        ///     Searches for a User by the given name
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task object that represents the User</returns>
        /// <exception cref="KeyNotFoundException">if no User with the given name exists.</exception>
        UserModel GetByName(string name);

        ///<summary>
        ///     Checks whether an User exists by the given id.
        ///</summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException">if no User with the given id exists.</exception>
        Task<bool> ExistsByIdAsync(string id);

        ///<summary>
        ///     Checks whether an User exists by the given name.
        ///</summary>
        /// <param name="username"></param>
        /// <exception cref="KeyNotFoundException">if no User with the given name exists.</exception>
        bool ExistsByName(string username);

        /// <summary>
        ///     Creates the specified User in the Users table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>A Task object that represents the id of the created User</returns>
        Task<string> CreateAsync(UserModel entity);

        /// <summary>
        ///     Overwrites the User's current properties with the new ones.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the edit.</returns>
        /// <exception cref="KeyNotFoundException">if no User with the given id exists.</exception>
        Task EditAsync(string id, UserModel model);

        /// <summary>
        ///     Deletes the User with the given id.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A Task object that represents the asynchronous Save Changes operation after the deletion.</returns>
        /// <exception cref="KeyNotFoundException">if no User with the given id exists.</exception>
        Task DeleteAsync(string username);
    }
}
