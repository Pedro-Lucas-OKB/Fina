namespace Fina.Api.Shared.Api;

public interface IEndpoint
{
    static abstract void Map(IEndpointRouteBuilder app);
}
