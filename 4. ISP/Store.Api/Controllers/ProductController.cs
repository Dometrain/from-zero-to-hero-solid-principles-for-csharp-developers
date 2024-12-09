using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Contracts.Requests;
using Store.Api.Mappers;
using Store.Application.Models;
using Store.Common.Results;
using Store.Application.Services;
using Store.Common.Helpers;
using FluentValidation;

namespace Store.Api.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class ProductController : BaseController<Product>
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService.NotNull();
    }

    [HttpGet]
    public async Task<IResult> GetProductsAsync([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _productService.GetProductsAsync(request.Page, request.PageSize, cancellationToken);
        return result switch
        {
            SuccessResult<Paged<Product>> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<Paged<Product>> => HandleNotFound(),
            _ => HandleUnknown()
        };
    }


    [HttpGet("{productId}")]
    public async Task<IResult> GetProductAsync(int productId, CancellationToken cancellationToken = default)
    {
        var result = await _productService.GetProductAsync(productId, cancellationToken);
        return result switch
        {
            SuccessResult<Product> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<Product> => HandleNotFound(),
            InvalidResult<Product> invalidResult => HandleInvalid(invalidResult),
            ErrorResult<Product> errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }
}
