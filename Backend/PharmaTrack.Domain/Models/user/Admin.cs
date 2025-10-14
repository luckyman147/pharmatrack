using System;

namespace PharmaTrack.Domain.Models.user;

public class Admin : User
{
    public string PermissionsLevel { get; set; } = "Super";
        public Admin(   Guid id, string code, string fullName, string email, string passwordHash, string permissionsLevel)
    {
        this.id = id;
        Code = code;
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        PermissionsLevel = permissionsLevel;
        EmailConfirmed = false;
    }
}
