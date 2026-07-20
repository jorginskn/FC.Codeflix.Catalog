using FC.Codeflix.Catalog.Api.Configurations;
using FC.Codeflix.Catalog.Api.Configurations;
using FC.Codeflix.Catalog.Infra.Data.EF.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUseCases();
builder.Services.AddInfra();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConnections(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
