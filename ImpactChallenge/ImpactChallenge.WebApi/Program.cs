using ImpactChallenge.WebApi.ApiClients;
//using ImpactChallenge.WebApi.Filters;
using ImpactChallenge.WebApi.Middleware;
using ImpactChallenge.WebApi.repository;
using ImpactChallenge.WebApi.Services;
using ImpactChallenge.WebApi.Utils;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

ConfigureApp(builder);

void ConfigureServices(IServiceCollection services)
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

    services.AddControllers(
        //options =>
        //{
        //    options.Filters.Add(typeof(ExceptionFilter));
        //    options.Filters.Add(typeof(ExecutionTimeFilter));
        //}
    );

    services.AddLogging(builder =>
    {
        builder.AddConsole();
    });

    services
        .AddScoped<IProductService, ProductService>()
        .AddScoped<IOrderService, OrderService>()
        .AddSingleton(configuration)
        .AddScoped<IConfigurationHelper, ConfigurationHelper>()
        .AddTransient<IBasketApiClient, BasketApiClient>()
        .AddSingleton<IBasketRepo, BasketRepo>()
        .AddHttpClient()
        ;

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

void ConfigureApp(WebApplicationBuilder builder)
{
    

    var app = builder.Build();

    app.UseLoggingMiddleware();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    //app.UseAuthorization();

    app.MapControllers();

    app.Run();
}