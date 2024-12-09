using Store.Application.Mappers;
using Store.Application.Models;
using Store.Common.Results;
using Store.Infrastructure.Data;

namespace Store.Application.Services;

public class OrderService
{
    private readonly OrderRepository _orderRepository;
    private readonly CartRepository _cartRepository;

    public OrderService()
    {
        _orderRepository = new OrderRepository();
        _cartRepository = new CartRepository();
    }

    public async Task<Result<Order>> CreateOrderAsync(int userId, int cartId, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartAsync(userId, cartId, cancellationToken);
        if (cart == null)
            return new InvalidResult<Order>("InvalidCart", new[] { new Error("missing_cart", $"Cart with id {cartId} does not exist.") });

        var newOrder = new Order
        {
            UserId = userId,
            Items = cart.Items.Select(x => x.Map()),
            DeliveryCost = 3.99m,
            Tax = 0
        };

        var orderId = await _orderRepository.CreateOrderAsync(newOrder.Map(), cancellationToken);
        if (orderId == null)
            return new ErrorResult<Order>("Unexpected error occurred creating order");

        var order = await _orderRepository.GetOrderAsync(userId, orderId.Value, cancellationToken);
        return new SuccessResult<Order>(order.Map());
    }

    public async Task<Result<Order>> GetOrderAsync(int userId, int orderId, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderAsync(userId, orderId, cancellationToken);
        if (order == null)
            return new NotFoundResult<Order>();

        return new SuccessResult<Order>(order.Map());
    }

    public async Task<Result<Paged<Order>>> GetOrdersAsync(int userId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrdersAsync(userId, page, pageSize, cancellationToken);
        return new SuccessResult<Paged<Order>>(orders.Map());
    }
}