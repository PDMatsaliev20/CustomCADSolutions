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
        private readonly IConverter converter;
        private readonly IOrderService orderService;

        public CadService(IRepository repository, IConverter converter,
            IOrderService orderService)
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

            await repository.AddAsync<Cad>(cad);
            await repository.SaveChangesAsync();

            return cad.Id;
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

            cad.CategoryId = model.CategoryId;
            cad.Name = model.Name;
            cad.Validated = model.Validated;
            cad.X = model.Coords.Item1;
            cad.Y = model.Coords.Item2;
            cad.Z = model.Coords.Item3;
            cad.CreationDate = model.CreationDate;
            cad.SpinAxis = model.SpinAxis;
            cad.SpinFactor = model.SpinFactor;
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
                ?? throw new ArgumentException("Model doesn't exist!");

                cad.CategoryId = model.CategoryId;
                cad.Name = model.Name;
                cad.Validated = model.Validated;
                cad.X = model.Coords.Item1;
                cad.Y = model.Coords.Item2;
                cad.Z = model.Coords.Item3;
                cad.CreationDate = model.CreationDate;
                cad.SpinAxis = model.SpinAxis;
                cad.SpinFactor = model.SpinFactor;
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
            Cad? cad = await this.repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException();

            cad.CreatorId = default;
            cad.Creator = default;
            cad.CreationDate = default;
            cad.Validated = default;
            
            // Cad animation info
            cad.X = default;
            cad.Y = default;
            cad.Z = default;
            cad.SpinAxis = default;
            cad.SpinFactor = default;

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

        public async Task<CadModel> GetByIdAsync(int id)
        {
            Cad? cad = await repository.GetByIdAsync<Cad>(id)
                ?? throw new KeyNotFoundException($"Model with id: {id} doesn't exist");
            CadModel model = converter.CadToModel(cad);
            return model;
        }

        public async Task<IEnumerable<CadModel>> GetAllAsync()
        {
            return await repository
                .All<Cad>()
                .Select(cad => converter.CadToModel(cad, true))
                .ToArrayAsync();
        }
    }
}
