using Store.Application.Mappers;
using Store.Application.Models;
using Store.Common.Results;
using Store.Infrastructure.Data;

namespace Store.Application.Services;

public class CartService : ICartService
{
    private readonly IReadCart _readCart;
    private readonly IWriteCart _writeCart;
    private readonly IReadProducts _readProducts;

    public CartService(IReadCart readCart, IWriteCart writeCart, IReadProducts readProducts)
    {
        _readCart = readCart ?? throw new ArgumentNullException(nameof(readCart));
        _writeCart = writeCart ?? throw new ArgumentNullException(nameof(writeCart));
        _readProducts = readProducts ?? throw new ArgumentNullException(nameof(readProducts));
    }

    public async Task<Result<Cart>> AddToCartAsync(int userId, int cartId, Item item, CancellationToken cancellationToken)
    {
        // Validate Cart Exists
        var cart = await _readCart.GetCartAsync(userId, cartId, cancellationToken);
        if (cart == null)
            return new InvalidResult<Cart>("InvalidCart", new[] { new Error("missing_cart", $"Shopping cart with id {cartId} does not exist.") });

        // Validate Product Exists
        var product = await _readProducts.GetProductAsync(item.Product.ProductId, cancellationToken);
        if (product == null)
            return new InvalidResult<Cart>("InvalidProduct", new[] { new Error("missing_product", $"Product Id {item.Product.ProductId} does not exist.") });

        var result = await _writeCart.AddToCartAsync(cartId, item.Map(), cancellationToken);
        if (!result)
            return new ErrorResult<Cart>("An unexpected error occured adding item to shopping cart");

        cart = await _readCart.GetCartAsync(userId, cartId, cancellationToken);

        return new SuccessResult<Cart>(cart.Map());
    }

    public async Task<Result<Cart>> CreateCartAsync(int userId, Cart cart, CancellationToken cancellationToken)
    {
        // Validate Products Exist
        var errors = new List<Error>();
        foreach (var item in cart.Items)
        {
            var product = await _readProducts.GetProductAsync(item.Product.ProductId, cancellationToken);
            if (product == null)
            {
                errors.Add(new Error("InvalidProduct", $"Product Id {item.Product.ProductId} does not exist."));
            }
        }

        if (errors.Any())
            return new InvalidResult<Cart>("InvalidCart", errors);

        var cartId = await _writeCart.CreateCartAsync(userId, cart.Map(), cancellationToken);
        if (cartId == null)
            return new ErrorResult<Cart>("An unexpected error occurred creating shopping cart");

        var cartResult = await _readCart.GetCartAsync(userId, cartId.Value, cancellationToken);

        return new SuccessResult<Cart>(cartResult.Map());
    }

    public async Task<Result<Cart>> GetCartAsync(int userId, int cartId, CancellationToken cancellationToken)
    {
        var result = await _readCart.GetCartAsync(userId, cartId, cancellationToken);
        if (result == null)
            return new NotFoundResult<Cart>();

        return new SuccessResult<Cart>(result.Map());
    }

    public async Task RemoveCartAsync(int userId, int cartId, CancellationToken cancellationToken)
    {
        await _writeCart.RemoveCartAsync(userId, cartId, cancellationToken);
    }

    public async Task RemoveFromCartAsync(int userId, int cartId, int itemId, CancellationToken cancellationToken)
    {
        await _writeCart.RemoveFromCartAsync(userId, cartId, itemId, cancellationToken);
    }
}