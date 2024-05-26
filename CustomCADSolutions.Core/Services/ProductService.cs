using AutoMapper;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Mappings;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Core.Services
{
    public class ProductService(IRepository repository) : IProductService
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductCoreProfile>();
            cfg.AddProfile<CadCoreProfile>();

        }).CreateMapper();

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            Product[] products = await repository.AllReadonly<Product>().ToArrayAsync();

            var models = mapper.Map<IEnumerable<ProductModel>>(products);
            return models;
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            Product product = await repository.GetByIdAsync<Product>(id)
                ?? throw new KeyNotFoundException($"Model with id: {id} doesn't exist.");

            ProductModel model = mapper.Map<ProductModel>(product);
            return model;
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await repository.GetByIdAsync<Product>(id) != null;

        public IList<string> ValidateEntity(OrderModel model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                return validationResults.Select(result => result.ErrorMessage ?? string.Empty).ToList();
            }

            return new List<string>();
        }

        public async Task<int> CreateAsync(ProductModel model)
        {
            Product product = mapper.Map<Product>(model);

            EntityEntry<Product> entry = await repository.AddAsync<Product>(product);
            await repository.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public async Task EditAsync(int id, ProductModel model)
        {
            Product product = await repository.GetByIdAsync<Product>(id)
                ?? throw new KeyNotFoundException("Model with id doesn't exist.");

            product.Name = model.Name;
            product.Price = model.Price;
            product.IsValidated = model.IsValidated;
            product.CategoryId = model.CategoryId;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Product product = await repository.GetByIdAsync<Product>(id)
                ?? throw new KeyNotFoundException();
            RevertOrders(product.Orders);

            repository.Delete<Product>(product);
            await repository.SaveChangesAsync();
        }

        private static void RevertOrders(IEnumerable<Order> orders)
        {
            foreach (Order order in orders)
            {
                order.Status = OrderStatus.Pending;
                order.ProductId = null;
                order.Product = null;
            }
        }
    }
}
