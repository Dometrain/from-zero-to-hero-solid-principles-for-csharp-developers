using Store.Application.Mappers;
using Store.Application.Models;
using Store.Common.Helpers;
using Store.Common.Results;
using Store.Infrastructure.Data;
using Store.Infrastructure.Models;

namespace Store.Application.Services;

public class OrderService : IOrderService
{
    private readonly IReadOrders _readOrders;
    private readonly IWriteOrder _writeOrder;
    private readonly IReadCart _readCart;
    private readonly ITaxCalculatorFactory _taxCalculatorFactory;
    private readonly IUserService _userService;

    public OrderService(IReadOrders readOrders, IWriteOrder writeOrder, IReadCart readCart, ITaxCalculatorFactory taxCalculatorFactory, IUserService userService)
    {
        _readOrders = readOrders.NotNull();
        _writeOrder = writeOrder.NotNull();
        _readCart = readCart.NotNull();
        _taxCalculatorFactory = taxCalculatorFactory.NotNull();
        _userService = userService.NotNull();
    }

    public async Task<Result<Order>> CreateOrderAsync(int userId, int cartId, CancellationToken cancellationToken)
    {
        var cart = await _readCart.GetCartAsync(userId, cartId, cancellationToken);
        if (cart == null)
            return new InvalidResult<Order>("InvalidCart", new[] { new Error("missing_cart", $"Cart with id {cartId} does not exist.") });

        var user = await _userService.GetUserAsync(userId, cancellationToken);
        var taxCalculator = _taxCalculatorFactory.GetCalculatorInstance(user.Data.CountryCode);

        var newOrder = new Order
        {
            UserId = userId,
            Items = cart.Items.Select(x => x.Map()),
            DeliveryCost = 3.99m
        };

        newOrder.Tax = taxCalculator.CalculateTax(newOrder, user.Data);

        var orderId = await _writeOrder.CreateOrderAsync(newOrder.Map(), cancellationToken);
        if (orderId == null)
            return new ErrorResult<Order>("Unexpected error occurred creating order");

        var order = await _readOrders.GetOrderAsync(userId, orderId.Value, cancellationToken);
        return new SuccessResult<Order>(order.Map());
    }

    public async Task<Result<Order>> GetOrderAsync(int userId, int orderId, CancellationToken cancellationToken)
    {
        var order = await _readOrders.GetOrderAsync(userId, orderId, cancellationToken);
        if (order == null)
            return new NotFoundResult<Order>();

        return new SuccessResult<Order>(order.Map());
    }

    public async Task<Result<Paged<Order>>> GetOrdersAsync(int userId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var orders = await _readOrders.GetOrdersAsync(userId, page, pageSize, cancellationToken);
        return new SuccessResult<Paged<Order>>(orders.Map());
    }
}