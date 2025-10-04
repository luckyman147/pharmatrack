using System;

namespace PharmaTrack.Application.Interfaces.services;

public interface IPasswordHasher
{
   string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
}
