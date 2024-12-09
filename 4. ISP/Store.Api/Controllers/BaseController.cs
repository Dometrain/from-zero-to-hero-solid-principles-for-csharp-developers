using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Store.Common.Results;

namespace Store.Api.Controllers;

public class BaseController<T> : ControllerBase where T : class
{
    protected IResult HandleSuccess(object data)
    {
        if (data == null) return HandleNoContent();
        return Results.Ok(data);
    }

    protected IResult HandleNoContent()
    {
        return Results.NoContent();
    }

    protected IResult HandleInvalid(InvalidResult<T> result)
    {
        return Results.ValidationProblem(result.Errors.ToDictionary(x => x.Code, x => new[] { x.Details }), result.Message);
    }

    protected IResult HandleErrors(ErrorResult<T> result)
    {
        return Results.Problem(result.Message);
    }

    protected IResult HandleErrors(ErrorResult result)
    {
        return Results.Problem(result.Message);
    }

    protected IResult HandleNotFound()
    {
        return Results.NotFound();
    }

    protected IResult HandleUnknown()
    {
        return Results.StatusCode(500);
    }

    protected int UserId => int.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
}