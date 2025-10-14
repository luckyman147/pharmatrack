using System;
using System.Data.Common;

namespace PharmaTrack.Domain.Models;

public class Delegue : User
{
    public decimal Salary { get; set; }
    public bool IsActive { get; set; }

    public Delegue(Guid id, string code, string fullName, string email, string passwordHash, decimal salary,bool isActive)
    {
        this.id = id;
        this.Code = code;
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = isActive;
        EmailConfirmed = false;
        this.Salary = salary;
    }
    
}
