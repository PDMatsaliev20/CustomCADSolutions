using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;

namespace CustomCADs.Application.Services
{
    public class CadService(IDbTracker dbTracker,
        IQueries<Cad, int> cadQueries,
        IQueries<Order, int> orderQueries,
        ICommands<Cad> commands,
        IMapper mapper) : ICadService
    {
        public async Task<CadResult> GetAllAsync(CadQuery query, SearchModel search, PaginationModel pagination, Func<Cad, bool>? customFilter = null)
        {
            IEnumerable<Cad> cads = await cadQueries.GetAll(
                user: query.Creator,
                status: query.Status.ToString(),
                category: search.Category,
                name: search.Name,
                owner: search.Owner,
                sorting: search.Sorting,
                customFilter,
                asNoTracking: true
            ).ConfigureAwait(false);

            CadModel[] models = mapper.Map<CadModel[]>(cads
                .Skip((pagination.Page - 1) * pagination.Limit)
                .Take(pagination.Limit)
            );

            return new()
            {
                Count = cads.Count(),
                Cads = models,
            };
        }

        public async Task<CadModel> GetByIdAsync(int id)
        {
            Cad cad = await cadQueries.GetByIdAsync(id, true).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Model with id: {id} doesn't exist");

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
                ?? throw new KeyNotFoundException("No such Cad exists.");

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
                ?? throw new KeyNotFoundException("Model doesn't exist!");

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
            IEnumerable<Order> orders = await orderQueries.GetAll(
                customFilter: o => o.CadId == id
            ).ConfigureAwait(false);

            foreach (Order order in orders)
            {
                order.Status = OrderStatus.Pending;
                order.DesignerId = null;
                order.Designer = null;
                order.CadId = null;
                order.Cad = null;
            }
            
            Cad cad = await cadQueries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            commands.Delete(cad);
            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
