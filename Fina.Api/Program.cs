using Fina.Api;
using Fina.Api.Endpoints;
using Fina.Api.Shared.Api;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfigurations();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ConfigureDevEnvironment();
}

app.UseCors(ApiConfiguration.CorsPolicyName);
app.MapEndpoints();

app.Run();
