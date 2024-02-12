using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.Core.Services
{
    public class CadService : ICadService
    {
        private readonly IRepository repository;
        private readonly IOrderService orderService;

        public CadService(IRepository repository, IOrderService orderService)
        {
            this.repository = repository;
            this.orderService = orderService;

        }

        public async Task CreateAsync(CadModel model)
        {
            Cad cad = new()
            {
                Name = model.Name,
                Category = model.Category,
                CadInBytes = model.CadInBytes,
                CreationDate = DateTime.Now,
                CreatorId = model.CreatorId,
                Creator = model.Creator,
            };

            if (model.Orders.Any())
            {
                await orderService.CreateRangeAsync(model.Orders.ToArray());
            }

            await repository.AddAsync<Cad>(cad);
            await repository.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(params CadModel[] models)
        {
            List<Cad> cads = new();
            foreach (CadModel model in models)
            {
                Cad cad = new()
                {
                    Name = model.Name,
                    CreationDate = DateTime.Now,
                    Category = model.Category,
                    CadInBytes = model.CadInBytes,
                    CreatorId = model.CreatorId,
                    Creator = model.Creator,
                };

                if (model.Orders.Any())
                {
                    await orderService.CreateRangeAsync(model.Orders.ToArray());
                }

                cads.Add(cad);
            }
            await repository.AddRangeAsync<Cad>(cads.ToArray());
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Cad? cad = await this.repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException("Cad not found");

            repository.Delete<Cad>(cad);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(params int[] ids)
        {
            Cad[] cads = (await Task.WhenAll(
                    ids.Select(async id => await repository.GetByIdAsync<Cad>(id))))
                .Where(result => result != null)
                .ToArray()!;

            repository.DeleteRange<Cad>(cads);
            await repository.SaveChangesAsync();
        }

        public async Task EditAsync(CadModel entity)
        {
            Cad? cad = await this.repository.All<Cad>()
                .FirstOrDefaultAsync(cad => cad.Id == entity.Id)
                ?? throw new ArgumentException("Model doesn't exist!");

            cad.Name = entity.Name;
            cad.Category = entity.Category;
            cad.CadInBytes = entity.CadInBytes;

            await repository.SaveChangesAsync();
        }

        public async Task<CadModel> GetByIdAsync(int id)
        {
            Cad cad = await repository
                .All<Cad>()
                .FirstOrDefaultAsync(cad => cad.Id == id)
                ?? throw new ArgumentException("Model doesn't exist");

            CadModel model = new()
            {
                Id = cad.Id,
                Name = cad.Name,
                CreationDate = cad.CreationDate,
                Category = cad.Category,
                CadInBytes = cad.CadInBytes,
                CreatorId = cad.CreatorId,
                Creator = cad.Creator,
            };

            if (cad.Orders.Any())
            {
                model.Orders = cad.Orders
                    .Select(o => orderService.GetByIdAsync(o.CadId, o.BuyerId).Result)
                    .ToArray();
            }

            return model;
        }

        public async Task<IEnumerable<CadModel>> GetAllAsync()
        {
            CadModel[] models = await repository
                .All<Cad>()
                .Select(cad => new CadModel()
                {
                    Id = cad.Id,
                    Name = cad.Name,
                    Category = cad.Category,
                    CreationDate = cad.CreationDate,
                    CadInBytes = cad.CadInBytes,
                    CreatorId = cad.CreatorId,
                    Creator = cad.Creator,
                    Orders = cad.Orders.Select(o => orderService.GetByIdAsync(o.CadId, o.BuyerId).Result).ToArray(),
                })
                .ToArrayAsync();

            return models;
        }
    }
}
