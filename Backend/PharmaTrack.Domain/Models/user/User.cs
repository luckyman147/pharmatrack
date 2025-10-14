using System;
using MongoDB.Bson.Serialization.Attributes;
using PharmaTrack.Domain.Abstractions;
using PharmaTrack.Domain.Models.user;
using PharmaTrack.Domain.ValueObject;

namespace PharmaTrack.Domain.Models;

    [BsonKnownTypes(typeof(Admin), typeof(Delegue))]
public abstract class User : Entity<Guid>
{
    public string Code { get;  set; }           // login code / unique identifier
    public string FullName { get;  set; }
    public string Email { get;  set; }
    public string PasswordHash { get; set; }
    public string Role => GetType().Name;

    public bool EmailConfirmed { get;  set; }

    public UserProfile Profile { get;  set; }
    // For Mongo you may want a parameterless ctor
    public User() { }

    public User(Guid id, string code, string fullName, string email, string passwordHash, string role = "Delegue")
    {
        this.id = id;
        Code = code;
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
  
        EmailConfirmed = false;
    }

    public void ConfirmEmail() => EmailConfirmed = true;
    public void UpdatePassword(string newHash) => PasswordHash = newHash;
       public void SetProfile(UserProfile profile) => Profile = profile;
    }


