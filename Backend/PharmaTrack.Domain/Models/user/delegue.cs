using System;
using System.Data.Common;

namespace PharmaTrack.Domain.Models;

public class Delegue : User
{
    public decimal Salary { get; set; }
    public decimal Commission { get; set; }
    public bool IsActive { get; set; }

    public Delegue(Guid id, string code, string fullName, string email, string passwordHash, decimal salary, decimal commission, bool isActive)
        : base(id, code, fullName, email, passwordHash, "Delegue")
    {
        Salary = salary;
        Commission = commission;
        IsActive = isActive;
    }
}
