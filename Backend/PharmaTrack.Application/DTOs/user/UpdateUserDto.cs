using System;
using AutoMapper;
using PharmaTrack.Domain.Models.user;

namespace PharmaTrack.Application.DTOs.user;

public class UpdateUserDto
{
public Guid Id { get; set; }
public UserProfile userProfile { get; set; }
}
