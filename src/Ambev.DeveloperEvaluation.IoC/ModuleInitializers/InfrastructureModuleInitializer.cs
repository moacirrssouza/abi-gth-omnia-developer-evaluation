using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Caching;
using Ambev.DeveloperEvaluation.ORM.EventStore;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISaleRepository, SaleRepository>();

        // Redis
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp => 
            ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379"));
        builder.Services.AddScoped<ICacheService, RedisCacheService>();

        // MongoDB
        builder.Services.AddSingleton<IMongoClient>(sp => 
            new MongoClient(builder.Configuration.GetConnectionString("MongoDB") ?? "mongodb://localhost:27017"));
        builder.Services.AddScoped<IMongoDatabase>(sp => 
            sp.GetRequiredService<IMongoClient>().GetDatabase("DeveloperEvaluation"));
        builder.Services.AddScoped<IEventStore, MongoEventStore>();
    }
}