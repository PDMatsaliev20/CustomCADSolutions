using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Helpers;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Services;

public class UserService(
    IUserQueries queries,
    ICommands<User> commands,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IUserService
{
    private const string UserNotFoundMessage = "The User with {0}: {1} does not exist.";

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
        User entity = await queries.GetByIdAsync(id, true).ConfigureAwait(false)
            ?? throw new UserNotFoundException(string.Format(UserNotFoundMessage, "id", id));
        
        UserModel model = mapper.Map<UserModel>(entity);
        return model;
    }

    public async Task<UserModel> GetByNameAsync(string name)
    {
        User user = await queries.GetByNameAsync(name, true).ConfigureAwait(false)
            ?? throw new UserNotFoundException(string.Format(UserNotFoundMessage, "name", name));

        UserModel model = mapper.Map<UserModel>(user);
        return model;
    }
    
    public async Task<UserModel> GetByRefreshToken(string rt)
    {
        User? user = await queries.GetByRefreshTokenAsync(rt).ConfigureAwait(false)
            ?? throw new UserNotFoundException(string.Format(UserNotFoundMessage, "refresh token", rt));

        UserModel model = mapper.Map<UserModel>(user);
        return model;
    }

    public async Task<bool> ExistsByIdAsync(string id)
        => await queries.ExistsByIdAsync(id).ConfigureAwait(false);

    public async Task<bool> ExistsByName(string username)
        => await queries.ExistsByNameAsync(username).ConfigureAwait(false);

    public async Task<string> CreateAsync(UserModel model)
    {
        User user = mapper.Map<User>(model);
        User addedUser = await commands.AddAsync(user).ConfigureAwait(false);

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        return addedUser.Id;
    }

    public async Task EditAsync(string username, UserModel model)
    {
        User? entity = await queries.GetByNameAsync(username).ConfigureAwait(false)
            ?? throw new UserNotFoundException(string.Format(UserNotFoundMessage, "username", username));

        entity.UserName = model.UserName;
        entity.FirstName = model.FirstName;
        entity.LastName = model.LastName;
        entity.Email = model.Email;
        entity.RoleName = model.RoleName;
        entity.RefreshToken = model.RefreshToken;
        entity.RefreshTokenEndDate = model.RefreshTokenEndDate;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(string username)
    {
        User? user = await queries.GetByNameAsync(username).ConfigureAwait(false)
            ?? throw new UserNotFoundException(string.Format(UserNotFoundMessage, "username", username));


        commands.Delete(user);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
