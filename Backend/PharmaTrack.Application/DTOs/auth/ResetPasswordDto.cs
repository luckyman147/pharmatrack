using System;

namespace PharmaTrack.Application.DTOs;

public class ResetPasswordDto { public string Identifier { get; set; } = null!; public string Code { get; set; } = null!; public string NewPassword { get; set; } = null!; }