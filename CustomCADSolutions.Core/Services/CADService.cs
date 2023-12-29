using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CreationDate = entity.CreationDate,
                Customer = entity.Customer,
                Author = entity.Author,
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
            cad.Description = entity.Description;
            cad.CreationDate = entity.CreationDate;
            cad.Customer = entity.Customer;
            cad.Author = entity.Author;

            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<CADModel>> GetAll()
        {
            return await repository
                .All<CAD>()
                .Select(cad => new CADModel
                {
                    Id = cad.Id,
                    Name = cad.Name,
                    Description = cad.Description,
                    CreationDate = cad.CreationDate,
                    Customer = cad.Customer,
                    Author = cad.Author,
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
                Description = cad.Description,
                CreationDate = cad.CreationDate,
                Customer = cad.Customer,
                Author = cad.Author,
            };

            return model;
        }
    }
}
