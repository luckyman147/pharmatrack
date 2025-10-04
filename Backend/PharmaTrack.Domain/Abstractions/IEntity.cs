using System;

namespace PharmaTrack.Domain.Abstractions;
    public interface IEntity<T> : IEntity
    {
        public T id { get; set; }
    }
    public interface IEntity
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy{ get; set; }
        public DateTime? LastModified{ get; set; }
        public string? LastModifiedBy{ get; set; }
    }