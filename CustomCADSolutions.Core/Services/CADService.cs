using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Crmf;

namespace CustomCADSolutions.Core.Services
{
    public class CADService : ICADService
    {
        private readonly IRepository repository;

        public CADService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task CreateAsync(CadModel entity)
        {
            Cad cad = new()
            {
                Name = entity.Name,
                CreationDate = DateTime.Now,
                Category = entity.Category,
                Url = entity.Url,
                Orders = entity.Orders
                    .Select(o => new Order 
                    {
                        Description = o.Description,
                        OrderDate = o.OrderDate 
                    })
                    .ToArray()
            };

            await repository.AddAsync<Cad>(cad);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Cad? cad = await repository.GetByIdAsync<Cad>(id);

            if (cad != null)
            {
                repository.Delete<Cad>(cad);
            }
        }

        public async Task EditAsync(CadModel entity)
        {
            Cad? cad = await repository
                .All<Cad>()
                .FirstOrDefaultAsync(cad => cad.Id == entity.Id)
                ?? throw new ArgumentException("Model doesn't exist!");

            cad.Id = entity.Id;
            cad.Name = entity.Name;
            cad.CreationDate = entity.CreationDate;
            cad.Category = entity.Category;
            cad.Url = entity.Url;

            await repository.SaveChangesAsync();
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
    }
}
