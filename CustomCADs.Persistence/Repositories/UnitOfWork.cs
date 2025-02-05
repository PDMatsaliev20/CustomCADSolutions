﻿using CustomCADs.Domain.Shared;

namespace CustomCADs.Persistence.Repositories;

public class UnitOfWork(ApplicationContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();
}
