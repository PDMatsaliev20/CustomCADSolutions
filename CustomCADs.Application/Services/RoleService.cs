using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Helpers;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Services
{
    public class RoleService(
        IRoleQueries queries, 
        ICommands<Role> commands, 
        IDbTracker tracker, 
        IMapper mapper) : IRoleService
    {
        public RoleResult GetAll(string? name = null, string? description = null, string sorting = "", int page = 1, int limit = 50, Func<RoleModel, bool>? customFilter = null)
        {
            IQueryable<Role> queryable = queries.GetAll(true);
            queryable = queryable.Filter(customFilter == null ? null : r => customFilter(mapper.Map<RoleModel>(r)) );
            queryable = queryable.Search(name, description);
            queryable = queryable.Sort(sorting);

            IEnumerable<Role> roles = [..queryable.Skip((page - 1) * limit).Take(limit)];
            return new()
            {
                Count = queryable.Count(),
                Roles = mapper.Map<RoleModel[]>(roles),
            };
        }

        public async Task<RoleModel> GetByIdAsync(string id) 
        {
            Role? role = await queries.GetByIdAsync(id).ConfigureAwait(false);
            ArgumentNullException.ThrowIfNull(role);

            return mapper.Map<RoleModel>(role);
        }

        public async Task<RoleModel> GetByNameAsync(string name)
        {
            Role? role = await queries.GetByNameAsync(name);
            ArgumentNullException.ThrowIfNull(role);

            return mapper.Map<RoleModel>(role);
        }

        public async Task<bool> ExistsByIdAsync(string id)
            => await queries.ExistsByIdAsync(id).ConfigureAwait(false);
        
        public async Task<bool> ExistsByNameAsync(string name)
            => await queries.ExistsByNameAsync(name).ConfigureAwait(false);

        public async Task<string> CreateAsync(RoleModel model)
        {
            Role role = mapper.Map<Role>(model);
            
            await commands.AddAsync(role);
            await tracker.SaveChangesAsync().ConfigureAwait(false);

            return role.Id;
        }

        public async Task EditAsync(string name, RoleModel model)
        {
            IQueryable<Role> roles = queries.GetAll(true);
            roles = roles.Filter(customFilter: r => r.Name == name);

            Role role = roles.SingleOrDefault() ?? throw new KeyNotFoundException();
            role.Name = model.Name;
            role.Description = model.Description;

            await tracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(string name)
        {
            IQueryable<Role> roles = queries.GetAll(true);
            roles = roles.Filter(customFilter: r => r.Name == name);

            Role role = roles.SingleOrDefault() ?? throw new KeyNotFoundException();
            commands.Delete(role);

            await tracker.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
