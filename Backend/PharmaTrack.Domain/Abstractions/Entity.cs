using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PharmaTrack.Domain.Abstractions;

    public abstract class Entity<T> : IEntity<T>
    {
          [BsonId] 
    [BsonRepresentation(BsonType.String)] 
        public T id { get; set; } = default!;
        public  DateTime? CreatedAt { get; set; }= DateTime.UtcNow;
        public  string? CreatedBy { get; set; }
        public  DateTime? LastModified { get; set; }= DateTime.UtcNow;
        public  string? LastModifiedBy { get; set; }
    }
