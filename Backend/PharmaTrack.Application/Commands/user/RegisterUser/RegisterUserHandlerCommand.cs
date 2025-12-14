using System;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Application.Interfaces;
using PharmaTrack.Application.Interfaces.services;
using PharmaTrack.Domain.Models;
using PharmaTrack.Domain.Models.user;
using PharmaTrack.Domain.ValueObject;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.user.RegisterUser;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand, RegisterUserResult>
{
  
    private readonly ILogger<RegisterUserHandler> _logger;        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;
        private readonly IEmailService _email;

    public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher hasher, IEmailService email, ILogger<RegisterUserHandler> logger)
    {
        _userRepository = userRepository;
        _hasher = hasher;
        _email = email;
                _logger = logger;
        }

        public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
      var dto = request.user;

        _logger.LogInformation("Registering user with FullName: {FullName}, Email: {Email}", 
            dto.FullName, dto.Email);

        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
        {
            _logger.LogWarning("Registration attempt failed: Email {Email} already in use", dto.Email);
            throw new Exception("Email already in use");
        }
   var existingAdmin = await _userRepository.GetAdminAsync();
            // Generate unique user code
            var code = "U" + Guid.NewGuid().ToString("N")[..6].ToUpper();

            // Hash password
            var hash = _hasher.HashPassword(dto.Password);

           User user;

            if (existingAdmin == null)
            {
                // First registered user becomes Admin
                user = new Admin(id: Guid.NewGuid(),
                                 code: code,
                                 fullName: dto.FullName,
                                 email: dto.Email,
                                 passwordHash: hash,
                                 permissionsLevel: "Super");

                _logger.LogInformation("No admin found — assigning this user as the system Admin.");
            }
            else
            {
                // All next users are Delegues by default
                user = new Delegue(
                    id: Guid.NewGuid(),
                    code: code,
                    fullName: dto.FullName,
                    email: dto.Email,
                    isActive:true,
                    passwordHash: hash,
                    commission: 7,
                    salary:  0
                );

                _logger.LogInformation("User registered as Delegue.");
            }

            await _userRepository.AddAsync(user);

            _logger.LogInformation("User {Email} registered successfully with Code: {Code}", user.Email, user.Code);

            // Optionally send welcome email
            // await _email.SendEmailAsync(user.Email, "Welcome", $"Welcome {user.FullName}, your code: {user.Code}");

            return new RegisterUserResult(user.Code);
        
            // Send welcome/confirmation email
      //todo:later
      //       await _email.SendEmailAsync(user.Email, "Welcome", $"Welcome {user.FullName}, your code: {user.Code}");

        
        }
    
}
