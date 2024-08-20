using Microsoft.AspNetCore.Mvc;
using Optimizely.Demo.Core.Models.Api;
using Optimizely.Demo.PublicWeb.Api.Requests;
using Optimizely.Demo.PublicWeb.Api.Responses;
using Optimizely.Demo.PublicWeb.Api.Services;
using System.Globalization;

namespace Optimizely.Demo.PublicWeb.Api;

public static class Endpoints
{
    public static IEndpointRouteBuilder AddApiEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api");
        group.MapGet("/subpage", Get)
            .Produces<ResponseList<SubPage>>();
        group.MapGet("/subpage/{id}", GetById)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ResponseSingle<SubPage>>();
        group.MapPost("/post", Post)
            .Produces(StatusCodes.Status201Created);
        //.RequireAuthorization();

        return builder;
    }

    private static IResult Post([FromBody] PostRequest request, HttpContext context)
    {
        return Results.Created(request.Id.ToString(), request);
    }

    private static IResult Get(IContentLoader contentLoader, IApiService apiService, HttpContext context)
    {
        var subPages = contentLoader
            .GetChildren<PageData>(ContentReference.StartPage, CultureInfo.GetCultureInfo("sr"))
            .Select(x => new SubPage { Name = x.Name });
        var response = new ResponseList<SubPage>
        {
            Results = subPages
        };

        return Results.Ok(response);
    }

    private static IResult GetById(int id, IContentLoader contentLoader, HttpContext context)
    {
        var subPage = contentLoader
            .GetChildren<PageData>(ContentReference.StartPage, CultureInfo.GetCultureInfo("sr"))
            .FirstOrDefault();
        var response = new ResponseSingle<SubPage>
        {
            Result = new SubPage { Name = subPage?.Name }
        };

        return Results.Ok(response);
    }
}
