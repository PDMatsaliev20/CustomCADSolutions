using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Drawing;
using AutoMapper;
using CustomCADSolutions.Core.Mappings;


namespace CustomCADSolutions.Core.Services
{
    public class CadService : ICadService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public CadService(IRepository repository)
        {
            this.repository = repository;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CadProfile>();
                cfg.AddProfile<OrderProfile>();
            }).CreateMapper();
        }

        public async Task<int> CreateAsync(CadModel model)
        {
            Cad cad = mapper.Map<Cad>(model);
            EntityEntry<Cad> entry = await repository.AddAsync<Cad>(cad);
            await repository.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public async Task CreateRangeAsync(params CadModel[] models)
        {
            Cad[] cads = mapper.Map<Cad[]>(models);
            await repository.AddRangeAsync(cads);
            await repository.SaveChangesAsync();
        }

        public async Task EditAsync(CadModel model)
        {
            Cad cad = await repository.All<Cad>()
                .FirstOrDefaultAsync(cad => cad.Id == model.Id)
                ?? throw new ArgumentException("Model doesn't exist!");

            cad.Name = model.Name;
            cad.IsValidated = model.IsValidated;
            cad.Price = model.Price;
            cad.CreationDate = model.CreationDate;
            cad.SpinAxis = model.SpinAxis;

            cad.X = model.Coords[0];
            cad.Y = model.Coords[1];
            cad.Z = model.Coords[2];
            cad.R = model.Color.R;
            cad.G = model.Color.G;
            cad.B = model.Color.B;

            cad.CategoryId = model.CategoryId;
            cad.CreatorId = model.CreatorId;
            cad.Category = model.Category;
            cad.Creator = model.Creator;
            cad.Orders = mapper.Map<List<Order>>(model.Orders);

            await repository.SaveChangesAsync();
        }

        public async Task EditRangeAsync(params CadModel[] models)
        {
            CadModel[] newModels = models;
            foreach (CadModel model in models)
            {
                Cad cad = await repository.All<Cad>()
                .FirstOrDefaultAsync(cad => cad.Id == model.Id)
                ?? throw new KeyNotFoundException("Model doesn't exist!");

                cad.Name = model.Name;
                cad.IsValidated = model.IsValidated;
                cad.Price = model.Price;
                cad.CreationDate = model.CreationDate;
                cad.SpinAxis = model.SpinAxis;

                cad.X = model.Coords[0];
                cad.Y = model.Coords[1];
                cad.Z = model.Coords[2];
                cad.R = model.Color.R;
                cad.G = model.Color.G;
                cad.B = model.Color.B;

                cad.CategoryId = model.CategoryId;
                cad.CreatorId = model.CreatorId;
                cad.Category = model.Category;
                cad.Creator = model.Creator;
                cad.Orders = mapper.Map<Order[]>(model.Orders);
            }

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            (await repository.All<Order>().Where(o => o.CadId == id).ToListAsync())
                .ForEach(o => o.Status = OrderStatus.Pending);

            Cad cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException();

            cad.IsValidated = false;
            cad.CreatorId = null;
            cad.Creator = null;
            cad.CreationDate = null;
            cad.Bytes = null;
            cad.SpinAxis = null;
            cad.X = cad.Y = cad.Z = 0;
            cad.R = cad.G = cad.B = 0;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(params int[] ids)
        {
            (await repository.All<Order>().Where(o => ids.Contains(o.CadId)).ToListAsync())
                .ForEach(o => o.Status = OrderStatus.Pending);

            (await repository.All<Cad>().Where(c => ids.Contains(c.Id)).ToListAsync())
                .ForEach(cad =>
                {
                    cad.IsValidated = false;
                    cad.CreatorId = null;
                    cad.Creator = null;
                    cad.CreationDate = null;
                    cad.Bytes = null;
                    cad.SpinAxis = null;
                    cad.X = cad.Y = cad.Z = 0;
                    cad.R = cad.G = cad.B = 0;
                });

            await repository.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await repository.GetByIdAsync<Cad>(id) != null;

        public async Task<CadModel> GetByIdAsync(int id)
        {
            Cad cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException($"Model with id: {id} doesn't exist");

            CadModel model = mapper.Map<CadModel>(cad);
            return model;
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
    }
}
