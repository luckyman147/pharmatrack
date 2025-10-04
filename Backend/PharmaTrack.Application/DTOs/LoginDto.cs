namespace PharmaTrack.Application.DTOs;

public class LoginDto
{
        public string Code { get; set; } = null!; // code or full name or email
        public string Password { get; set; } = null!;
        public string? IpAddress { get; set; }

        public
        LoginDto(string code, string password, string ipAddress)
        {
        Code = code;
        Password = password;
        IpAddress = ipAddress;
    }
        
        
}
