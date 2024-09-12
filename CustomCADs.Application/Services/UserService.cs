using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Helpers;
using CustomCADs.Application.Models.Users;
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
        public UserResult GetAll(bool? hasRT, string? username, string? email, string? firstName, string? lastName, DateTime? rtEndDateBefore, DateTime? rtEndDateAfter, string sorting = "", int page = 1, int limit = 20, Func<UserModel, bool>? customFilter = null)
        {
            IQueryable<User> queryable = queries.GetAll(true);
            queryable = queryable.Filter(hasRT, customFilter != null ? u => customFilter(mapper.Map<UserModel>(u)) : null);
            queryable = queryable.Search(username, email, firstName, lastName, rtEndDateBefore, rtEndDateAfter);
            queryable = queryable.Sort(sorting);

            IEnumerable<User> entities = [.. queryable.Skip((page - 1) * limit).Take(limit)];
            return new()
            {
                Count = queryable.Count(),
                Users = mapper.Map<UserModel[]>(entities),
            };
        }

        public async Task<UserModel> GetByIdAsync(string id)
        {
            User? entity = await queries.GetByIdAsync(id).ConfigureAwait(false);
            ArgumentNullException.ThrowIfNull(entity);

            UserModel model = mapper.Map<UserModel>(entity);
            return model;
        }

        public UserModel GetByName(string name)
        {
            IQueryable<User> users = queries.GetAll(true);
            users = users.Filter(customFilter: u => u.UserName == name);

            UserModel model = mapper.Map<UserModel>(users.Single());
            return model;
        }

        public async Task<bool> ExistsByIdAsync(string id)
            => await queries.ExistsByIdAsync(id).ConfigureAwait(false);

        public bool ExistsByName(string username)
        {
            IQueryable<User> users = queries.GetAll(true);
            users = users.Filter(customFilter: u => u.UserName == username);

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
            IQueryable<User> users = queries.GetAll(true);
            users = users.Filter(customFilter: u => u.UserName == username);
            
            commands.Delete(users.Single());
            await tracker.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
