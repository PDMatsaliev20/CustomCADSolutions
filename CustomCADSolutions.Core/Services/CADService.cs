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
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public CadService(IRepository repository, IOrderService orderService)
        {
            this.repository = repository;
            this.orderService = orderService;
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
            cad.CreationDate = model.CreationDate;
            cad.SpinAxis = model.SpinAxis;
            
            cad.X = model.Coords.Item1;
            cad.Y = model.Coords.Item2;
            cad.Z = model.Coords.Item3;
            cad.R = model.Color.R;
            cad.G = model.Color.G;
            cad.B = model.Color.B;
            
            cad.CategoryId = model.CategoryId;
            cad.CreatorId = model.CreatorId;
            cad.Category = model.Category;
            cad.Creator = model.Creator;
            cad.Orders = mapper.Map<Order[]>(model.Orders);

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
                cad.CreationDate = model.CreationDate;
                cad.SpinAxis = model.SpinAxis;

                cad.X = model.Coords.Item1;
                cad.Y = model.Coords.Item2;
                cad.Z = model.Coords.Item3;
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
            List<OrderModel> orders = (await orderService.GetAllAsync())
                .Where(o => o.CadId == id).ToList();

            orders.ForEach(o => o.Status = OrderStatus.Pending);
            await orderService.EditRangeAsync(orders.ToArray());

            Cad cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException();

            repository.Delete(cad);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(params int[] ids)
        {
            List<OrderModel> orders = (await orderService.GetAllAsync())
                    .Where(o => ids.Contains(o.CadId)).ToList();

            orders.ForEach(o => o.Status = OrderStatus.Pending);
            await orderService.EditRangeAsync(orders.ToArray());

            Cad[] cads = await repository.All<Cad>()
                .Where(c => ids.Contains(c.Id))
                .ToArrayAsync();

            repository.DeleteRange(cads);
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

        public async Task<CadQueryModel> GetAllAsync(
            string? category = null, string? creator = null,
            string? searchName = null, string? searchCreator = null,
            CadSorting sorting = CadSorting.Newest,
            int currentPage = 1, int cadsPerPage = 1,
            bool validated = true, bool unvalidated = false)
        {
            IQueryable<Cad> allCads = repository.All<Cad>()
                .Where(c => c.Bytes != null && c.CreatorId != null);

            if (category != null)
            {
                allCads = allCads.Where(c => c.Category.Name == category);
            }

            if (creator != null)
            {
                allCads = allCads.Where(c => c.Creator!.UserName == creator);
            }

            if (validated ^ unvalidated)
            {
                if (validated)
                {
                    allCads = allCads.Where(c => c.IsValidated);
                }

                if (unvalidated)
                {
                    allCads = allCads.Where(c => !c.IsValidated);
                }

            }

            if (searchName != null)
            {
                allCads = allCads.Where(c => c.Name.Contains(searchName));
            }
            if (searchCreator != null)
            {
                allCads = allCads.Where(c => c.Creator!.UserName.Contains(searchCreator));
            }

            allCads = sorting switch
            {
                CadSorting.Newest => allCads.OrderBy(c => c.CreationDate),
                CadSorting.Oldest => allCads.OrderByDescending(c => c.CreationDate),
                CadSorting.Alphabetical => allCads.OrderBy(c => c.Name),
                CadSorting.Unalphabetical => allCads.OrderByDescending(c => c.Name),
                CadSorting.Category => allCads.OrderBy(m => m.Category.Name),
                _ => allCads.OrderBy(c => c.Id),
            };


            if (cadsPerPage > 16)
            {
                cadsPerPage = 16;
            }

            Cad[] pageCads = await allCads
                .Skip((currentPage - 1) * cadsPerPage)
                .Take(cadsPerPage)
                .ToArrayAsync();


            CadModel[] models = mapper.Map<CadModel[]>(pageCads);
            return new()
            {
                TotalCount = allCads.Count(),
                CadModels = models,
            };
        }
    }
}
