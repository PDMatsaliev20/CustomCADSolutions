using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Infrastructure.Data.Common;
using CustomCADs.Infrastructure.Data.Models;
using CustomCADs.Infrastructure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AutoMapper;
using CustomCADs.Core.Mappings;
using System.Drawing;
using System.ComponentModel.DataAnnotations;


namespace CustomCADs.Core.Services
{
    public class CadService(IRepository repository) : ICadService
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CadCoreProfile>();
                cfg.AddProfile<OrderCoreProfile>();
            }).CreateMapper();

        public async Task<CadQueryResult> GetAllAsync(CadQueryModel query)
        {
            if (query.CadsPerPage >= 3)
            {
                // So that the cads in a page are always 1, 2, 3 or a multiple of 3
                query.CadsPerPage -= query.CadsPerPage % 3;
            }
            IQueryable<Cad> allCads = repository.All<Cad>();

            if (query.Category != null)
            {
                allCads = allCads.Where(c => c.Category.Name == query.Category);
            }

            if (query.Creator != null)
            {
                allCads = allCads.Where(c => c.Creator!.UserName == query.Creator);
            }

            if (query.Validated ^ query.Unvalidated)
            {
                if (query.Validated)
                {
                    allCads = allCads.Where(c => c.IsValidated);
                }

                if (query.Unvalidated)
                {
                    allCads = allCads.Where(c => !c.IsValidated);
                }
            }
            else
            {
                if (!(query.Validated && query.Unvalidated))
                {
                    allCads = allCads.Take(0);
                }
            }

            if (!string.IsNullOrWhiteSpace(query.SearchName))
            {
                allCads = allCads.Where(c => c.Name.Contains(query.SearchName));
            }
            if (!string.IsNullOrWhiteSpace(query.SearchCreator))
            {
                allCads = allCads.Where(c => c.Creator.UserName!.Contains(query.SearchCreator));
            }

            allCads = query.Sorting switch
            {
                CadSorting.Newest => allCads.OrderBy(c => c.CreationDate),
                CadSorting.Oldest => allCads.OrderByDescending(c => c.CreationDate),
                CadSorting.Alphabetical => allCads.OrderBy(c => c.Name),
                CadSorting.Unalphabetical => allCads.OrderByDescending(c => c.Name),
                CadSorting.Category => allCads.OrderBy(m => m.Category.Name),
                _ => allCads.OrderBy(c => c.Id),
            };

            if (query.CadsPerPage > 16)
            {
                query.CadsPerPage = 16;
            }

            Cad[] cads = await allCads
                .Skip((query.CurrentPage - 1) * query.CadsPerPage)
                .Take(query.CadsPerPage)
                .ToArrayAsync();

            CadModel[] models = mapper.Map<CadModel[]>(cads);
            return new()
            {
                Count = (await allCads.ToArrayAsync()).Length,
                Cads = models,
            }; 
        }

        public async Task<CadModel> GetByIdAsync(int id)
        {
            Cad cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException($"Model with id: {id} doesn't exist");

            CadModel model = mapper.Map<CadModel>(cad);
            return model;
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await repository.GetByIdAsync<Cad>(id) != null;

        public int Count(Func<CadModel, bool> predicate)
        {
            return repository.Count<Cad>(cad => predicate(mapper.Map<CadModel>(cad)));
        }

        public async Task SetPathAsync(int id, string path)
        {
            Cad cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException("No such Cad exists.");

            cad.Path = path;
            await repository.SaveChangesAsync();
        }

        public IList<string> ValidateEntity(CadModel model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                return validationResults.Select(result => result.ErrorMessage ?? string.Empty).ToList();
            }

            return new List<string>();
        }

        public async Task<int> CreateAsync(CadModel model)
        {
            Cad cad = mapper.Map<Cad>(model);
            EntityEntry<Cad> entry = await repository.AddAsync<Cad>(cad);
            await repository.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public async Task EditAsync(int id, CadModel model)
        {
            Cad cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException("Model doesn't exist!");

            cad.Name = model.Name;
            cad.IsValidated = model.IsValidated;
            cad.Price = model.Price;
            cad.CategoryId = model.CategoryId;

            cad.X = model.Coords[0];
            cad.Y = model.Coords[1];
            cad.Z = model.Coords[2];
            cad.PanX = model.PanCoords[0];
            cad.PanY = model.PanCoords[1];
            cad.PanZ = model.PanCoords[2];

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            IEnumerable<Order> orders = repository.All<Order>()
                .Where(o => o.CadId == id);

            foreach (Order order in orders)
            {
                order.Status = OrderStatus.Pending;
                order.CadId = null;
                order.Cad = null;
            }

            Cad cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException();

            repository.Delete<Cad>(cad);
            await repository.SaveChangesAsync();
        }
    }
}
