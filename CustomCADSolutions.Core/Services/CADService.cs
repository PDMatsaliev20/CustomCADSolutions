using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AutoMapper;
using CustomCADSolutions.Core.Mappings;
using System.Drawing;
using System.ComponentModel.DataAnnotations;


namespace CustomCADSolutions.Core.Services
{
    public class CadService : ICadService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public CadService(IRepository repository)
        {
            this.repository = repository;
            MapperConfiguration config = new(cfg =>
            {
                cfg.AddProfile<CadCoreProfile>();
                cfg.AddProfile<OrderCoreProfile>();
            });
            this.mapper = config.CreateMapper();
        }

        public async Task<CadQueryModel> GetAllAsync(CadQueryModel query)
        {
            IQueryable<Cad> allCads = repository.All<Cad>().Where(c => c.Bytes != null);

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

            if (query.LikeName != null)
            {
                allCads = allCads.Where(c => c.Name.Contains(query.LikeName));
            }
            if (query.LikeCreator != null)
            {
                allCads = allCads.Where(c => c.Creator!.UserName.Contains(query.LikeCreator));
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
                TotalCount = allCads.Count(),
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

        public async Task ChangeColorAsync(int id, Color color)
        {
            Cad cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException("Model doesn't exist");

            cad.R = color.R;
            cad.G = color.G;
            cad.B = color.B;

            await repository.SaveChangesAsync();
        }

        public int Count(Func<CadModel, bool> predicate)
        {
            return repository.Count<Cad>(cad => predicate(mapper.Map<CadModel>(cad)));
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

            cad.SpinAxis = model.SpinAxis;
            cad.X = model.Coords[0];
            cad.Y = model.Coords[1];
            cad.Z = model.Coords[2];

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
