using ConsumidorPedidos.Config;
using ConsumidorPedidos.Config.Ioc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Get the database connection string from appsettings.json or environment variable
string? sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Check if the connection string is available
if (string.IsNullOrEmpty(sqlConnection))
{
    throw new InvalidOperationException("Database connection string is not configured.");
}

builder.Services.ConfigureDatabase(sqlConnection!);
builder.Services.UpdateMigrationDatabase();

// Add IOC
builder.Services.ConfigureRepositoryIoc();
builder.Services.ConfigureMessagingIoc();
builder.Services.ConfigureServiceIoc();

// Add HealthCheck
builder.Services.AddHealthChecks();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ConsumidorPedidosAPI", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); 

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
    _ = endpoints.MapGet("/", async context =>
    {
        var data = new { message = "Bem-vindo à ConsumidorPedidos API" };
        await context.Response.WriteAsJsonAsync(data);
    });

    _ = endpoints.MapHealthChecks("/health");
});

app.Run();
