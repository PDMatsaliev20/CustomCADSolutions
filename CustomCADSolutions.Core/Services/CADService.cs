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

        public async Task CreateAsync(CADModel entity)
        {
            CAD cad = new()
            {
                Name = entity.Name,
                CreationDate = DateTime.Now,
                Category = entity.Category,
                Url = entity.Url,
            };

            await repository.AddAsync<CAD>(cad);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            CAD? cad = await repository.GetByIdAsync<CAD>(id);

            if (cad != null)
            {
                repository.Delete<CAD>(cad);
            }
        }

        public async Task EditAsync(CADModel entity)
        {
            CAD? cad = await repository
                .All<CAD>()
                .FirstOrDefaultAsync(cad => cad.Id == entity.Id)
                ?? throw new ArgumentException("Model doesn't exist!");

            cad.Id = entity.Id;
            cad.Name = entity.Name;
            cad.CreationDate = entity.CreationDate;
            cad.Category = entity.Category;
            cad.Url = entity.Url;

            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<CADModel>> GetAllAsync()
        {
            return await repository
                .All<CAD>()
                .Select(cad => new CADModel
{
                    Id = cad.Id,
                    Name = cad.Name,
                    CreationDate = cad.CreationDate,
                    Category = cad.Category,
                    Url = cad.Url,
                })
                .ToListAsync();
        }

        public async Task<CADModel> GetByIdAsync(int id)
        {
            CAD cad = await repository
                .All<CAD>()
                .FirstOrDefaultAsync(cad => cad.Id == id)
                ?? throw new ArgumentException("Model doesn't exist");

            CADModel model = new()
            {
                Id = cad.Id,
                Name = cad.Name,
                CreationDate = cad.CreationDate,
                Category = cad.Category,
                Url = cad.Url,
            };

            return model;
        }

        public IEnumerable<Category> GetCategories()
        {
            return repository.All<Category>().Include(c => c.CADs);
        }
    }
}
