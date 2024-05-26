using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AutoMapper;
using CustomCADSolutions.Core.Mappings;
using System.ComponentModel.DataAnnotations;


namespace CustomCADSolutions.Core.Services
{
    public class CadService(IRepository repository) : ICadService
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CadCoreProfile>();
            cfg.AddProfile<OrderCoreProfile>();
        }).CreateMapper();

        public async Task<CadQueryModel> GetAllAsync(CadQueryModel query)
        {
            IQueryable<Cad> all = repository.All<Cad>();

            if (query.Category != null)
            {
                all = all.Where(c => c.Product.Category.Name == query.Category);
            }

            if (query.Creator != null)
            {
                all = all.Where(c => c.Creator.UserName == query.Creator);
            }

            if (query.Validated ^ query.Unvalidated)
            {
                if (query.Validated)
                {
                    all = all.Where(c => c.Product.IsValidated);
                }

                if (query.Unvalidated)
                {
                    all = all.Where(c => !c.Product.IsValidated);
                }
            }
            else
            {
                if (!(query.Validated && query.Unvalidated))
                {
                    all = all.Take(0);
                }
            }

            if (query.SearchName != null)
            {
                all = all.Where(c => c.Product.Name.Contains(query.SearchName));
            }
            if (query.SearchCreator != null)
            {
                all = all.Where(c => c.Creator.UserName!.Contains(query.SearchCreator));
            }

            all = query.Sorting switch
            {
                CadSorting.Newest => all.OrderBy(c => c.CreationDate),
                CadSorting.Oldest => all.OrderByDescending(c => c.CreationDate),
                CadSorting.Alphabetical => all.OrderBy(c => c.Product.Name),
                CadSorting.Unalphabetical => all.OrderByDescending(c => c.Product.Name),
                CadSorting.Category => all.OrderBy(m => m.Product.Category.Name),
                _ => all.OrderBy(c => c.Id),
            };

            if (query.CadsPerPage > 16)
            {
                query.CadsPerPage = 16;
            }

            Cad[] cads = await all
                .Skip((query.CurrentPage - 1) * query.CadsPerPage)
                .Take(query.CadsPerPage)
                .ToArrayAsync();

            CadModel[] models = mapper.Map<CadModel[]>(cads);
            return new()
            {
                TotalCount = all.Count(),
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

        public int Count(Predicate<CadModel> predicate)
        {
            return repository.Count<Cad>(cad => predicate(mapper.Map<CadModel>(cad)));
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

            cad.X = model.Coords[0];
            cad.Y = model.Coords[1];
            cad.Z = model.Coords[2];

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Cad cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException();

            repository.Delete<Cad>(cad);
            await repository.SaveChangesAsync();
        }
    }
}
