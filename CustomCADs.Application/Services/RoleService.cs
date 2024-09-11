using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Services
{
    public class RoleService(IQueries<Role, string> queries, ICommands<Role> commands, IDbTracker tracker, IMapper mapper) : IRoleService
    {
        public async Task<RoleResult> GetAllAsync(SearchModel search, PaginationModel pagination, Func<Role, bool>? customFilter = null)
        {
            IEnumerable<Role> role = await queries.GetAll().ConfigureAwait(false);

            RoleModel[] models = mapper.Map<RoleModel[]>(role
                .Skip((pagination.Page - 1) * pagination.Limit)
                .Take(pagination.Limit)
            );

            return new()
            {
                Count = role.Count(),
                Roles = models,
            };
        }

        public async Task<RoleModel> GetByNameAsync(string name)
        {
            IEnumerable<Role> roles = await queries.GetAll(customFilter: r => r.Name == name).ConfigureAwait(false);
            Role role = roles.SingleOrDefault() ?? throw new KeyNotFoundException();
            
            RoleModel model = mapper.Map<RoleModel>(role);
            return model;
        }

        public async Task<bool> ExistsByIdAsync(string id)
            => await queries.ExistsByIdAsync(id).ConfigureAwait(false);
        
        public async Task<bool> ExistsByNameAsync(string name)
        {
            IEnumerable<Role> roles = await queries.GetAll(customFilter: r => r.Name == name).ConfigureAwait(false);
            return roles.Count() == 1;
        }

        public async Task<string> CreateAsync(RoleModel model)
        {
            Role role = mapper.Map<Role>(model);
            
            await commands.AddAsync(role);
            await tracker.SaveChangesAsync().ConfigureAwait(false);

            return role.Id;
        }

        public async Task EditAsync(string name, RoleModel model)
        {
            IEnumerable<Role> roles = await queries.GetAll(customFilter: r => r.Name == name).ConfigureAwait(false);
            Role role = roles.SingleOrDefault() ?? throw new KeyNotFoundException();

            role.Name = model.Name;
            role.Description = model.Description;
            await tracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(string name)
        {
            IEnumerable<Role> roles = await queries.GetAll(customFilter: r => r.Name == name).ConfigureAwait(false);
            Role role = roles.SingleOrDefault() ?? throw new KeyNotFoundException();
            
            commands.Delete(role);
            await tracker.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
