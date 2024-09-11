using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Persistence.Repositories.Users
{
    public class UserCommands(ApplicationContext context) : ICommands<User>
    {
        public async Task<User> AddAsync(User user)
            => (await context.Users.AddAsync(user).ConfigureAwait(false)).Entity;

        public async Task AddRangeAsync(params User[] users)
            => await context.Users.AddRangeAsync(users).ConfigureAwait(false);

        public void Delete(User user)
            => context.Remove(user);

        public void DeleteRange(params User[] users)
            => context.RemoveRange(users);
    }
}
