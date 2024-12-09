using Store.Application.Mappers;
using Store.Application.Models;
using Store.Common.Results;
using Store.Infrastructure.Data;

namespace Store.Application.Services;

public class CartService
{
    private readonly CartRepository _cartRepository;
    private readonly ProductRepository _productRepository;

    public CartService()
    {
        _cartRepository = new CartRepository();
        _productRepository= new ProductRepository();
    }

    public async Task<Result<Cart>> AddToCartAsync(int userId, int cartId, Item item, CancellationToken cancellationToken)
    {
        // Validate Cart Exists
        var cart = await _cartRepository.GetCartAsync(userId, cartId, cancellationToken);
        if (cart == null)
            return new InvalidResult<Cart>("InvalidCart", new[] { new Error("missing_cart", $"Shopping cart with id {cartId} does not exist.") });

        // Validate Product Exists
        var product = await _productRepository.GetProductAsync(item.Product.ProductId, cancellationToken);
        if (product == null)
            return new InvalidResult<Cart>("InvalidProduct", new[] { new Error("missing_product", $"Product Id {item.Product.ProductId} does not exist.") });

        var result = await _cartRepository.AddToCartAsync(cartId, item.Map(), cancellationToken);
        if (!result)
            return new ErrorResult<Cart>("An unexpected error occured adding item to shopping cart");

        cart = await _cartRepository.GetCartAsync(userId, cartId, cancellationToken);

        return new SuccessResult<Cart>(cart.Map());
    }

    public async Task<Result<Cart>> CreateCartAsync(int userId, Cart cart, CancellationToken cancellationToken)
    {
        // Validate Products Exist
        var errors = new List<Error>();
        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetProductAsync(item.Product.ProductId, cancellationToken);
            if (product == null)
            {
                errors.Add(new Error("InvalidProduct", $"Product Id {item.Product.ProductId} does not exist."));
            }
        }

        if (errors.Any())
            return new InvalidResult<Cart>("InvalidCart", errors);

        var cartId = await _cartRepository.CreateCartAsync(userId, cart.Map(), cancellationToken);
        if (cartId == null)
            return new ErrorResult<Cart>("An unexpected error occurred creating shopping cart");

        var cartResult = await _cartRepository.GetCartAsync(userId, cartId.Value, cancellationToken);

        return new SuccessResult<Cart>(cartResult.Map());
    }

    public async Task<Result<Cart>> GetCartAsync(int userId, int cartId, CancellationToken cancellationToken)
    {
        var result = await _cartRepository.GetCartAsync(userId, cartId, cancellationToken);
        if (result == null)
            return new NotFoundResult<Cart>();

        return new SuccessResult<Cart>(result.Map());
    }

    public async Task RemoveCartAsync(int userId, int cartId, CancellationToken cancellationToken)
    {
        await _cartRepository.RemoveCartAsync(userId, cartId, cancellationToken);
    }

    public async Task RemoveFromCartAsync(int userId, int cartId, int itemId, CancellationToken cancellationToken)
    {
        await _cartRepository.RemoveFromCartAsync(userId, cartId, itemId, cancellationToken);
    }
}