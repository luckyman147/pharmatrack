using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PharmaTrack.Domain.Exceptions;

namespace PharmaTrack.Domain.ValueObject;

public record class UserId
{
   
       public Guid Value { get; }
       private UserId(Guid value) => Value = value;
       public static UserId Of(Guid value)
       {
           ArgumentNullException.ThrowIfNull(value);
           if (value == Guid.Empty)
           {
               throw new DomainException("Order id cannot be empty");
           }
           return new UserId(value);
       }
   
}
