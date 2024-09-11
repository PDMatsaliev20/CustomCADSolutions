using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Services
{
    public class UserService(
        IQueries<User, string> queries,
        ICommands<User> commands,
        IDbTracker tracker,
        IMapper mapper) : IUserService
    {
        public async Task<UserResult> GetAllAsync(SearchModel search, PaginationModel pagination, Func<UserModel, bool>? customFilter = null)
        {
            IEnumerable<User> entities = await queries.GetAll().ConfigureAwait(false);
            UserModel[] models = mapper.Map<UserModel[]>(entities);

            return new()
            {
                Count = entities.Count(),
                Users = models,
            };
        }

        public async Task<UserModel> GetByIdAsync(string id)
        {
            User? entity = await queries.GetByIdAsync(id).ConfigureAwait(false);
            ArgumentNullException.ThrowIfNull(entity);
            
            UserModel model = mapper.Map<UserModel>(entity);
            return model;
        }
        
        public async Task<UserModel> GetByNameAsync(string name)
        {
            IEnumerable<User> users = await queries.GetAll(customFilter: u => u.UserName == name).ConfigureAwait(false);
            User entity = users.Single();
            
            UserModel model = mapper.Map<UserModel>(entity);
            return model;
        }
        
        public async Task<bool> ExistsByIdAsync(string id)
            => await queries.ExistsByIdAsync(id).ConfigureAwait(false);
        
        public async Task<bool> ExistsByNameAsync(string username)
        {
            IEnumerable<User> users = await queries.GetAll(customFilter: u => u.UserName == username).ConfigureAwait(false);
            return users.Count() == 1;
        }

        public async Task<string> CreateAsync(UserModel model)
        {
            User user = mapper.Map<User>(model);
            User addedUser = await commands.AddAsync(user).ConfigureAwait(false);
            
            await tracker.SaveChangesAsync().ConfigureAwait(false);
            return addedUser.Id;
        }

        public async Task EditAsync(string id, UserModel model)
        {
            User? entity = await queries.GetByIdAsync(id).ConfigureAwait(false);
            ArgumentNullException.ThrowIfNull(entity);

            entity.UserName = model.UserName;
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.RoleName = model.RoleName;
            entity.RefreshToken = model.RefreshToken;
            entity.RefreshTokenEndDate = model.RefreshTokenEndDate;

            await tracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(string username)
        {
            IEnumerable<User> result = await queries.GetAll(customFilter: u => u.UserName == username);
            User? user = result.Single();

            commands.Delete(user);
            await tracker.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
