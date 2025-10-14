using System;
using PharmaTrack.Domain.Models;
using PharmaTrack.Domain.Models.user;

namespace PharmaTrack.Application.Interfaces;

    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByCodeOrFullNameAsync(string identifier);
        Task<User?> GetByCodeAsync(string code);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
    Task<List<Delegue>> GetDeleguesAsync(bool?  isActive);
    Task<Admin?> GetAdminAsync();
}