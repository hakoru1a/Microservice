using AutoMapper;
using Constracts.Common.Interface;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;
using Product.API.Repository;
using Product.API.Repository.Interface;
namespace Product.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.ConfigureProductDbContext(configuration);
        services.AddInfrastructureServices();

        // Configure Swagger/OpenAPI
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ecom API",
                Version = "v1",
                Description = "",
                Contact = new OpenApiContact
                {
                    Name = "Chuong Dang",
                    Email = "hakoru1a@gmail.com",
                }
            });
        });
        return services;
    }
    private static IMapper AddMapper ()
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MapperProfile());
        });

        return mapperConfig.CreateMapper();

    }
    private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(name: "DefaultConnectionString");
        
        var builder = new MySqlConnectionStringBuilder(connectionString);

        services.AddDbContext<ProductContext>(optionsAction: m => m.UseMySql(builder.ConnectionString,
            ServerVersion.AutoDetect(builder.ConnectionString), mySqlOptionsAction: e =>
            {
                e.MigrationsAssembly("Product.API");
                e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            }));

        return services;
    }
    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
                      .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                      .AddScoped<IProductRepository, ProductRepository>().AddSingleton(AddMapper());
    }

}