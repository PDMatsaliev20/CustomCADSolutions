using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace CustomCADs.Application.Services
{
    public class CadService(IDbTracker dbTracker,
        IQueryRepository<Cad> cadQueries,
        IQueryRepository<Order> orderQueries, 
        ICommandRepository<Cad> commands,
        IMapper mapper) : ICadService
    {
        public async Task<CadResult> GetAllAsync(CadQuery query, SearchModel search, PaginationModel pagination, Expression<Func<Cad, bool>>? customFilter = null)
        {
            IQueryable<Cad> allCads = cadQueries.GetAll();

            // Querying
            if (query.Creator != null)
            {
                allCads = allCads.Where(c => c.Creator.UserName == query.Creator);
            }
            if (query.Status != null)
            {
                allCads = allCads.Where(c => c.Status == query.Status);
            }

            // Optional custom filter
            if (customFilter != null)
            {
                allCads = allCads.Where(customFilter);
            }

            // Search & Sort
            if (search.Category != null)
            {
                allCads = allCads.Where(c => c.Category.Name == search.Category);
            }
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                allCads = allCads.Where(c => c.Name.Contains(search.Name));
            }
            if (!string.IsNullOrWhiteSpace(search.Owner))
            {
                allCads = allCads.Where(c => c.Creator.UserName!.Contains(search.Owner));
            }

            allCads = search.Sorting.ToLower() switch
            {
                "newest" => allCads.OrderByDescending(c => c.CreationDate),
                "oldest" => allCads.OrderBy(c => c.CreationDate),
                "alphabetical" => allCads.OrderBy(c => c.Name),
                "unalphabetical" => allCads.OrderByDescending(c => c.Name),
                "category" => allCads.OrderBy(m => m.Category.Name),
                _ => allCads.OrderByDescending(c => c.Id),
            };

            Cad[] cads = await allCads
                .Skip((pagination.Page - 1) * pagination.Limit)
                .Take(pagination.Limit)
                .ToArrayAsync()
                .ConfigureAwait(false);

            CadModel[] models = mapper.Map<CadModel[]>(cads);
            return new()
            {
                Count = allCads.Count(),
                Cads = models,
            }; 
        }

        public async Task<CadModel> GetByIdAsync(int id)
        {
            Cad cad = await cadQueries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Model with id: {id} doesn't exist");

            CadModel model = mapper.Map<CadModel>(cad);
            return model;
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await cadQueries.GetByIdAsync(id).ConfigureAwait(false) != null;

        public int Count(Func<CadModel, bool> predicate)
        {
            return cadQueries.Count(cad => predicate(mapper.Map<CadModel>(cad)));
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
            IQueryable<Order> orders = orderQueries.GetAll()
                .Where(o => o.CadId == id);

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
