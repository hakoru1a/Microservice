﻿using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Common.Behaviours;
using Order.Application.Common.Mappings;
using Order.Domain.Interfaces;
using Order.Infrastructure.Repository;
using Ordering.Application.Common.Behaviours;

namespace Order.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
        services
            .AddSingleton(AddMapper())
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddTransient(serviceType: typeof(IPipelineBehavior<,>), implementationType: typeof(UnhandledExceptionBehaviour<,>))
            .AddTransient(serviceType: typeof(IPipelineBehavior<,>), implementationType: typeof(PerformanceBehaviour<,>))
            .AddTransient(serviceType: typeof(IPipelineBehavior<,>), implementationType: typeof(ValidationBehaviour<,>));
    private static IMapper AddMapper()
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        return mapperConfig.CreateMapper();

    }
}