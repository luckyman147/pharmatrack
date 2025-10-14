using System;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Application.Interfaces;
using PharmaTrack.Domain.Models;
using PharmaTrack.Domain.Models.user;
using PharmaTrack.Domain.ValueObject;

namespace PharmaTrack.Infrastructure.Repositories;

 public class UserRepository : IUserRepository
    {
        private readonly   IMongoCollection<User>  _users;
        public UserRepository(IApplicationDbContext context)
        {
            _users = context.Users;
        }

        public async Task AddAsync(User user) => await _users.InsertOneAsync(user);
        public async Task DeleteAsync(Guid id) => await _users.DeleteOneAsync(u => u.id == id);

    public async  Task<Admin?> GetAdminAsync()
    {
            return await _users.Find(Builders<User>.Filter.OfType<Admin>()).FirstOrDefaultAsync() as Admin;
    }

    public async Task<IEnumerable<User>> GetAllAsync() => await _users.Find(u => true).ToListAsync();
        public async Task<User?> GetByCodeAsync(string code) => await _users.Find(u => u.Code == code).FirstOrDefaultAsync();
        public async Task<User?> GetByCodeOrFullNameAsync(string identifier) =>
            await _users.Find(u => u.Code == identifier || u.FullName == identifier || u.Email == identifier).FirstOrDefaultAsync();
        public async Task<User?> GetByEmailAsync(string email) => await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        public async Task<User?> GetByIdAsync(Guid id) => await _users.Find(u => u.id == id).FirstOrDefaultAsync();

    public async Task<List<Delegue>> GetDeleguesAsync(bool? isActive)
    {
  var delegueCollection = _users.OfType<Delegue>();

    if (isActive.HasValue)
    {
        // Filter by IsActive
        var filter = Builders<Delegue>.Filter.Eq(d => d.IsActive, isActive.Value);
        return await delegueCollection.Find(filter).ToListAsync();
    }

    // If isActive is null, return all Delegues
    return await delegueCollection.Find(_ => true).ToListAsync();
    }

    public async Task UpdateAsync(User user) => await _users.ReplaceOneAsync(u => u.id == user.id, user);
    }