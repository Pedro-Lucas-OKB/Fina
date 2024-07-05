using Fina.Api.Endpoints.Categories;
using Fina.Api.Endpoints.Transactions;
using Fina.Api.Shared.Api;

namespace Fina.Api.Endpoints;

public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup(prefix:"");

        endpoints.MapGroup(prefix:"")
            .WithTags("Health Check")
            .MapGet("/", () => new { message =  "OK" });
        
        endpoints.MapGroup(prefix:"v1/categories")
            .WithTags("Categories")
            .MapEndpoint<CreateCategoryEndpoint>()
            .MapEndpoint<UpdateCategoryEndpoint>()
            .MapEndpoint<DeleteCategoryEndpoint>()
            .MapEndpoint<GetCategoryByIdEndpoint>()
            .MapEndpoint<GetAllCategoriesEndpoint>();
        
        endpoints.MapGroup(prefix:"v1/transactions")
            .WithTags("Transactions")
            .RequireAuthorization()
            .MapEndpoint<CreateTransactionEndpoint>()
            .MapEndpoint<UpdateTransactionEndpoint>()
            .MapEndpoint<DeleteTransactionEndpoint>()
            .MapEndpoint<GetTransactionByIdEndpoint>()
            .MapEndpoint<GetTransactionsByPeriodEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}
