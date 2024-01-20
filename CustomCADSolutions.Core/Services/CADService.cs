using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CustomCADSolutions.Core.Services
{
    public class CADService : ICADService
    {
        private readonly IRepository repository;

        public CADService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task CreateAsync(params CadModel[] models)
        {
            List<Cad> cads = new();
            foreach (CadModel model in models)
            {
                Cad cad = new()
                {
                    Name = model.Name,
                    CreationDate = DateTime.Now,
                    Category = model.Category,
                    Url = model.Url,
                    Orders = model.Orders?
                        .Select(o => new Order
                        {
                            Description = o.Description,
                            OrderDate = o.OrderDate
                        })
                        .ToArray()
                        ?? Array.Empty<Order>()
                };
                cads.Add(cad);
            }
            await this.repository.AddRangeAsync<Cad>(cads.ToArray());
            await this.repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Cad? cad = await this.repository.GetByIdAsync<Cad>(id);

            if (cad != null)
            {
                repository.Delete<Cad>(cad);
            }
        }

        public async Task EditAsync(CadModel entity)
        {
            Cad? cad = await this.repository
                .All<Cad>()
                .FirstOrDefaultAsync(cad => cad.Id == entity.Id)
                ?? throw new ArgumentException("Model doesn't exist!");

            cad.Id = entity.Id;
            cad.Name = entity.Name;
            cad.CreationDate = entity.CreationDate;
            cad.Category = entity.Category;
            cad.Url = entity.Url;

            await this.repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<CadModel>> GetAllAsync()
        {
            return await repository
                .All<Cad>()
                .Select(cad => new CadModel
                {
                    Id = cad.Id,
                    Name = cad.Name,
                    CreationDate = cad.CreationDate,
                    Category = cad.Category,
                    Url = cad.Url,
                })
                .ToListAsync();
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
                Url = cad.Url,
            };

            return model;
        }

        public async Task UpdateCads()
        {
            await repository.ResetDbAsync();
            string json = await File.ReadAllTextAsync("categories.json");
            CadModel[] cadDTOs = JsonSerializer.Deserialize<CadModel[]>(json)!;
            await CreateAsync(cadDTOs);
        }
    }
}
