﻿using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Shared;

namespace CustomCADs.Persistence.Repositories.Roles;

public class RoleCommands(ApplicationContext context) : ICommands<Role>
{
    public async Task<Role> AddAsync(Role role, CancellationToken ct)
        => (await context.Roles.AddAsync(role, ct).ConfigureAwait(false)).Entity;

    public async Task AddRangeAsync(params Role[] roles)
        => await context.Roles.AddRangeAsync(roles).ConfigureAwait(false);

    public void Delete(Role role)
        => context.Roles.Remove(role);

    public void DeleteRange(params Role[] roles)
        => context.Roles.RemoveRange(roles);
}
