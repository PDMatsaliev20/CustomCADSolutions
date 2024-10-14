using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Services
{
    public class OrderService(IUnitOfWork unitOfWork,
        IOrderQueries queries,
        ICommands<Order> commands,
        IMapper mapper) : IOrderService
    {
        public OrderResult GetAll(string? buyer = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", int page = 1, int limit = 20, Func<OrderModel, bool>? customFilter = null)
        {
            IQueryable<Order> queryable = queries.GetAll(true);
            queryable = queryable.Filter(buyer, status, customFilter == null ? null : o => customFilter(mapper.Map<OrderModel>(o)));
            queryable = queryable.Search(category, name, owner);
            queryable = queryable.Sort(sorting);

            IEnumerable<Order> entities = [..queryable.Skip((page - 1) * limit).Take(limit)];
            return new()
            {
                Count = queryable.Count(),
                Orders = mapper.Map<OrderModel[]>(entities)
            };
        }

        public async Task<OrderModel> GetByIdAsync(int id)
        {
            Order order = await queries.GetByIdAsync(id, asNoTracking: true)
                .ConfigureAwait(false)
                ?? throw new OrderNotFoundException(id);

            OrderModel model = mapper.Map<OrderModel>(order);
            return model;
        }
        
        public async Task<CadModel> GetCadAsync(int id)
        {
            Order? order = await queries.GetByIdAsync(id, asNoTracking: true)
                .ConfigureAwait(false);

            if (order == null || order.CadId == null)
            {
                throw new OrderMissingCadException(id);
            }

            CadModel model = mapper.Map<CadModel>(order.Cad);
            return model;
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await queries.ExistsByIdAsync(id).ConfigureAwait(false);

        public async Task<int> CountAsync(Func<OrderModel, bool> predicate)
        {
            return await queries.CountAsync(
                order => predicate(mapper.Map<OrderModel>(order)),
                asNoTracking: true
            ).ConfigureAwait(false);
        }
        
        public async Task SetImagePathAsync(int id, string imagePath)
        {
            Order? order = await queries.GetByIdAsync(id);
            ArgumentNullException.ThrowIfNull(order);

            order.ImagePath = imagePath;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> HasCadAsync(int id)
        {
            Order? order = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new OrderNotFoundException(id);
            return order.CadId != null;
        }
        
        public async Task<bool> CheckOwnership(int id, string username)
        {
            Order? order = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new OrderNotFoundException(id);
            return order.Buyer.UserName == username;
        }

        public async Task<int> CreateAsync(OrderModel model)
        {
            Order order = mapper.Map<Order>(model);

            await commands.AddAsync(order).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return order.Id;
        }

        public async Task EditAsync(int id, OrderModel model)
        {
            Order order = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new OrderNotFoundException(id);

            order.Name = model.Name;
            order.Description = model.Description;
            order.ShouldBeDelivered = model.ShouldBeDelivered;
            order.CategoryId = model.CategoryId;

            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            Order order = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new OrderNotFoundException(id);

            commands.Delete(order);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
