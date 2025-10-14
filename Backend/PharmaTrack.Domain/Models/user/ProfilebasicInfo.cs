using System;

namespace PharmaTrack.Domain.Models.user;

public class ProfilebasicInfo
{
public Guid Id { get; set; }
public required string fullName { get; set; }
    public string? avatarUrl { get; set; } = string.Empty;
}
