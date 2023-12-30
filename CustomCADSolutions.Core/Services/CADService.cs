using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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
                CreationDate = entity.CreationDate,
                Description = entity.Description,
                Orders = entity.Orders,
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
            cad.Description = entity.Description;
            cad.Orders = entity.Orders;

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
                    Description = cad.Description,
                    Orders = cad.Orders,
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
                Description = cad.Description,
                Orders = cad.Orders,
            };

            return model;
        }
    }
}
