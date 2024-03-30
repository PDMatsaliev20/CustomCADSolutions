using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Drawing;

namespace CustomCADSolutions.Core.Services
{
    public class CadService : ICadService
    {
        private readonly IRepository repository;
        private readonly IConverter converter;
        private readonly IOrderService orderService;

        public CadService(IRepository repository, IConverter converter, IOrderService orderService)
        {
            this.repository = repository;
            this.orderService = orderService;
            this.converter = converter;
        }

        public async Task<int> CreateAsync(CadModel model)
        {
            Cad cad = converter.ModelToCad(model);

            if (model.Orders.Any())
            {
                await orderService.CreateRangeAsync(model.Orders.ToArray());
            }

            EntityEntry<Cad> entry = await repository.AddAsync<Cad>(cad);
            await repository.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public async Task CreateRangeAsync(params CadModel[] models)
        {
            List<Cad> cads = new();
            foreach (CadModel model in models)
            {
                Cad cad = converter.ModelToCad(model);

                if (model.Orders.Any())
                {
                    await orderService.CreateRangeAsync(model.Orders.ToArray());
                }

                cads.Add(cad);
            }
            await repository.AddRangeAsync<Cad>(cads.ToArray());
            await repository.SaveChangesAsync();
        }

        public async Task EditAsync(CadModel model)
        {
            Cad cad = await repository.All<Cad>()
                .FirstOrDefaultAsync(cad => cad.Id == model.Id)
                ?? throw new ArgumentException("Model doesn't exist!");

            if (model.Color != Color.Empty)
            {
                cad.R = model.Color.R;
                cad.G = model.Color.G;
                cad.B = model.Color.B;
            }

            cad.Name = model.Name;
            cad.CategoryId = model.CategoryId;
            cad.IsValidated = model.IsValidated;
            cad.X = model.Coords.Item1;
            cad.Y = model.Coords.Item2;
            cad.Z = model.Coords.Item3;
            cad.CreationDate = model.CreationDate;
            cad.SpinAxis = model.SpinAxis;
            cad.CreatorId = model.CreatorId;
            cad.Creator = model.Creator;
            cad.Orders = model.Orders
                .Select(o => converter.ModelToOrder(o, false))
                .ToList();

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

                cad.CategoryId = model.CategoryId;
                cad.Name = model.Name;
                cad.IsValidated = model.IsValidated;
                cad.CreationDate = model.CreationDate;
                cad.SpinAxis = model.SpinAxis;

                cad.R = model.Color.R;
                cad.G = model.Color.G;
                cad.B = model.Color.B;
                cad.X = model.Coords.Item1;
                cad.Y = model.Coords.Item2;
                cad.Z = model.Coords.Item3;

                cad.CreatorId = model.CreatorId;
                cad.Creator = model.Creator;
                cad.Orders = model.Orders
                    .Select(o => converter.ModelToOrder(o, false))
                    .ToList();
            }

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Cad cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException();

            repository.Delete(cad);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(params int[] ids)
        {
            Cad[] cads = new Cad[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                cads[i] = await repository.GetByIdAsync<Cad>(ids[i])
                    ?? throw new KeyNotFoundException();
            }

            repository.DeleteRange(cads);
            await repository.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await repository.GetByIdAsync<Cad>(id) != null;

        public async Task<CadModel> GetByIdAsync(int id)
        {
            Cad cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException($"Model with id: {id} doesn't exist");

            CadModel model = converter.CadToModel(cad);
            return model;
        }

        public async Task<CadQueryModel> GetAllAsync(
            string? category = null, string? creator = null,
            string? searchName = null, string? searchCreator = null,
            CadSorting sorting = CadSorting.Newest,
            int currentPage = 1, int cadsPerPage = 1,
            bool validated = true, bool unvalidated = false)
        {
            IQueryable<Cad> cads = repository.All<Cad>()
                .Where(c => c.Bytes != null && c.CreatorId != null);

            if (category != null)
            {
                cads = cads.Where(c => c.Category.Name == category);
            }

            if (creator != null)
            {
                cads = cads.Where(c => c.Creator!.UserName == creator);
            }

            if (validated ^ unvalidated)
            {
                if (validated)
                {
                    cads = cads.Where(c => c.IsValidated);
                }

                if (unvalidated)
                {
                    cads = cads.Where(c => !c.IsValidated);
                }

            }

            if (searchName != null)
            {
                cads = cads.Where(c => c.Name.Contains(searchName));
            }
            if (searchCreator != null)
            {
                cads = cads.Where(c => c.Creator!.UserName.Contains(searchCreator));
            }

            cads = sorting switch
            {
                CadSorting.Newest => cads.OrderBy(c => c.CreationDate),
                CadSorting.Oldest => cads.OrderByDescending(c => c.CreationDate),
                CadSorting.Alphabetical => cads.OrderBy(c => c.Name),
                CadSorting.Unalphabetical => cads.OrderByDescending(c => c.Name),
                CadSorting.Category => cads.OrderBy(m => m.Category.Name),
                _ => cads.OrderBy(c => c.Id),
            };


            if (cadsPerPage > 16)
            {
                cadsPerPage = 16;
            }

            CadModel[] models = await cads
                .Skip((currentPage - 1) * cadsPerPage)
                .Take(cadsPerPage)
                .Select(cad => converter.CadToModel(cad, true))
                .ToArrayAsync();

            return new()
            {
                TotalCount = cads.Count(),
                CadModels = models
            };
        }
    }
}
