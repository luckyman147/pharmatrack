using System;
using PharmaTrack.Domain.Abstractions;
using PharmaTrack.Domain.Models.user;
using PharmaTrack.Domain.ValueObject;

namespace PharmaTrack.Domain.Models;

public class User : Aggregate<Guid>
{
    public string Code { get; private set; }           // login code / unique identifier
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; set; }
    public string Role { get; private set; }
    public bool EmailConfirmed { get; private set; }

    public UserProfile Profile { get; private set; }
    // For Mongo you may want a parameterless ctor
    public User() { }

    public User(Guid id, string code, string fullName, string email, string passwordHash, string role = "Delegue")
    {
        this.id = id;
        Code = code;
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        EmailConfirmed = false;
    }

    public void ConfirmEmail() => EmailConfirmed = true;
    public void UpdatePassword(string newHash) => PasswordHash = newHash;
       public void SetProfile(UserProfile profile) => Profile = profile;
    }


