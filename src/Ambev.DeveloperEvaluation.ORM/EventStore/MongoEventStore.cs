using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Domain.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.ORM.EventStore;

/// <summary>
/// MongoDB-based implementation of the domain event store.
/// Persists domain events in a dedicated collection for auditing or event-sourcing scenarios.
/// </summary>
public class MongoEventStore : IEventStore
{
    private readonly IMongoCollection<EventDocument> _collection;
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false
    };

    /// <summary>
    /// Initializes a new instance of the MongoEventStore class using the provided database.
    /// </summary>
    /// <param name="database">The MongoDB database where the events collection is stored.</param>
    public MongoEventStore(IMongoDatabase database)
    {
        _collection = database.GetCollection<EventDocument>("DomainEvents");
    }

    /// <summary>
    /// Persists a domain event document in MongoDB.
    /// </summary>
    /// <typeparam name="T">The event type implementing IDomainEvent.</typeparam>
    /// <param name="event">The event instance to persist.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    public async Task SaveEventAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IDomainEvent
    {
        var doc = new EventDocument
        {
            Id = Guid.NewGuid(),
            EventType = typeof(T).Name,
            Data = JsonSerializer.Serialize(@event, SerializerOptions),
            Timestamp = DateTime.UtcNow
        };
        await _collection.InsertOneAsync(doc, null, cancellationToken);
    }
}

/// <summary>
/// Represents a stored domain event document in MongoDB.
/// </summary>
public class EventDocument
{
    /// <summary>
    /// Unique identifier of this event document.
    /// </summary>
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    /// <summary>
    /// Event type name (notification class name).
    /// </summary>
    public string EventType { get; set; } = string.Empty;
    /// <summary>
    /// Event payload data serialized as a JSON string.
    /// </summary>
    public string Data { get; set; } = string.Empty;
    /// <summary>
    /// UTC timestamp when this event was stored.
    /// </summary>
    public DateTime Timestamp { get; set; }
}