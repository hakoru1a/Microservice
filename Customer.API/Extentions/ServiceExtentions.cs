﻿using AutoMapper;
using Constracts.Common.Interface;
using Customer.API.Persistence;
using Customer.API.Repository;
using Customer.API.Repository.Interface;
using Customer.API.Services;
using Customer.API.Services.Interface;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
namespace Customer.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.ConfigureCustomerDbContext(configuration);
        services.AddInfrastructureServices();
        services.ConfigureSwagger();

        return services;
    }
    private static IMapper AddMapper()
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MapperProfile());
        });

        return mapperConfig.CreateMapper();

    }
    private static IServiceCollection ConfigureCustomerDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString")
            ?? throw new ArgumentNullException(nameof(configuration), "Default connection string is not configured");

        services.AddDbContext<CustomerContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly("Customer.API");
            });
        });

        return services;
    }
    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
                      .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                      .AddScoped<ICustomerRepository, CustomerRepository>()
                      .AddScoped<ICustomerService, CustomerService>()
                      .AddSingleton(AddMapper());
    }
    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ecom API",
                Version = "v1",
                Description = "E-commerce API for Customer Management",  // Added meaningful description
                Contact = new OpenApiContact
                {
                    Name = "Chuong Dang",
                    Email = "hakoru1a@gmail.com",
                }
            });
        });
    }
}