using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CustomCADSolutions.Core.Services
{
    public class CadService : ICadService
    {
        private readonly IRepository repository;

        public CadService(IRepository repository)
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
                    CadInBytes = model.CadInBytes
                };

                if (model.Order != null)
                {
                    cad.Order = new Order
                    {
                        CadId = model.Order.CadId,
                        BuyerId = model.Order.BuyerId,
                        Description = model.Order.Description,
                        OrderDate = model.Order.OrderDate,
                    };
                }

                cads.Add(cad);
            }
            await this.repository.AddRangeAsync<Cad>(cads.ToArray());
            await this.repository.SaveChangesAsync();
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
            foreach (int id in ids)
            {
                Cad? cad = await this.repository.GetByIdAsync<Cad>(id);

                if (cad != null)
                {
                    repository.Delete<Cad>(cad);
                }
                await repository.SaveChangesAsync();
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
            cad.CadInBytes = entity.CadInBytes;

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
                    Category = cad.Category,
                    CreationDate = cad.CreationDate,
                    CadInBytes = cad.CadInBytes,
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
                CadInBytes = cad.CadInBytes,
            };

            return model;
        }
    }
}
