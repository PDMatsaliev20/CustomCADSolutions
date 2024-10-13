using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;

namespace CustomCADs.Application.Services
{
    public class CadService(IDbTracker dbTracker,
        ICadQueries cadQueries,
        IOrderQueries orderQueries,
        ICommands<Cad> commands,
        IMapper mapper) : ICadService
    {
        public CadResult GetAllAsync(string? creator = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", int page = 1, int limit = 20, Func<CadModel, bool>? customFilter = null)
        {
            IQueryable<Cad> queryable = cadQueries.GetAll(true);
            queryable = queryable.Filter(user: creator, status: status, customFilter: customFilter == null ? null : c => customFilter(mapper.Map<CadModel>(c)));
            queryable = queryable.Search(category: category, name: name, creator: owner);
            queryable = queryable.Sort(sorting: sorting);

            IEnumerable<Cad> cads = queryable.Skip((page - 1) * limit).Take(limit);
            return new()
            {
                Count = queryable.Count(),
                Cads = mapper.Map<CadModel[]>(cads),
            };
        }

        public async Task<CadModel> GetByIdAsync(int id)
        {
            Cad cad = await cadQueries.GetByIdAsync(id, true).ConfigureAwait(false)
                ?? throw new CadNotFoundException(id);

            CadModel model = mapper.Map<CadModel>(cad);
            return model;
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await cadQueries.ExistsByIdAsync(id).ConfigureAwait(false);

        public async Task<int> Count(Func<CadModel, bool> predicate)
        {
            return await cadQueries.CountAsync(
                cad => predicate(mapper.Map<CadModel>(cad)),
                asNoTracking: true
            ).ConfigureAwait(false);
        }

        public async Task SetPathsAsync(int id, string cadPath, string imagePath)
        {
            Cad cad = await cadQueries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new CadNotFoundException(id);

            cad.Paths = new(cadPath, imagePath);
            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> CreateAsync(CadModel model)
        {
            Cad cad = mapper.Map<Cad>(model);
            await commands.AddAsync(cad).ConfigureAwait(false);
            await dbTracker.SaveChangesAsync().ConfigureAwait(false);

            return cad.Id;
        }

        public async Task EditAsync(int id, CadModel model)
        {
            Cad cad = await cadQueries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new CadNotFoundException(id);

            cad.Name = model.Name;
            cad.Description = model.Description;
            cad.Price = model.Price;
            cad.CategoryId = model.CategoryId;

            cad.CamCoordinates = model.CamCoordinates;
            cad.PanCoordinates = model.PanCoordinates;

            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            IQueryable<Order> queryable = orderQueries.GetAll();
            queryable = queryable.Filter(customFilter: o => o.CadId == id);

            IEnumerable<Order> orders = [..queryable];
            foreach (Order order in orders)
            {
                order.Status = OrderStatus.Pending;
                order.DesignerId = null;
                order.Designer = null;
                order.CadId = null;
                order.Cad = null;
            }
            
            Cad cad = await cadQueries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new CadNotFoundException(id);

            commands.Delete(cad);
            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
