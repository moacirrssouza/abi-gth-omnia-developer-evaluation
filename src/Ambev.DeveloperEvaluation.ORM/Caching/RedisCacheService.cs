using Ambev.DeveloperEvaluation.Application.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.ORM.Caching;

/// <summary>
/// Redis-backed cache service implementation for storing and retrieving application data.
/// </summary>
public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;

    /// <summary>
    /// Initializes a new instance of RedisCacheService with the given Redis connection multiplexer.
    /// </summary>
    /// <param name="redis">The Redis connection multiplexer used to access databases.</param>
    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    /// <summary>
    /// Retrieves a value from cache by key and deserializes it to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the cached value into.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>The deserialized value or default if not found.</returns>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        var value = await db.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default;
        return JsonSerializer.Deserialize<T>(value!);
    }

    /// <summary>
    /// Serializes and stores a value in cache under the specified key, with optional expiration.
    /// </summary>
    /// <typeparam name="T">The type of the value to store.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="expiration">Optional expiration timespan; if omitted the value persists until evicted.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        var json = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, json);
        if (expiration.HasValue)
        {
            await db.KeyExpireAsync(key, expiration);
        }
    }

    /// <summary>
    /// Removes a value from cache by key.
    /// </summary>
    /// <param name="key">The cache key to remove.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(key);
    }
}