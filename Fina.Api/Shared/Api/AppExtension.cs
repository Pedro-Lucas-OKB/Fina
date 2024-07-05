namespace Fina.Api.Shared.Api;

public static class AppExtension
{
    public static void ConfigureDevEnvironment(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        //app.MapSwagger().RequireAuthorization();
    }
}
