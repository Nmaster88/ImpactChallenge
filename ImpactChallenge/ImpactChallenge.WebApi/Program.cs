using ImpactChallenge.WebApi.ApiClients;
using ImpactChallenge.WebApi.Filters;
using ImpactChallenge.WebApi.repository;
using ImpactChallenge.WebApi.Services;
using ImpactChallenge.WebApi.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
    options.Filters.Add(typeof(ExecutionTimeFilter));
});

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
});

builder.Services
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IOrderService, OrderService>()
            .AddSingleton(configuration)
            .AddScoped<IConfigurationHelper, ConfigurationHelper>()
            .AddTransient<IBasketApiClient, BasketApiClient>()
            .AddSingleton<IBasketRepo, BasketRepo>()
            .AddHttpClient()
            ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
