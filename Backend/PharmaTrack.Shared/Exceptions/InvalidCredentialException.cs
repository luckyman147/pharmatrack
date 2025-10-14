using System;

namespace PharmaTrack.Shared.Exceptions;

public class InvalidCredentialsException: Exception
    {
        public InvalidCredentialsException() 
            : base("Invalid credentials")
        {
        }

        public InvalidCredentialsException(string message) 
            : base(message)
        {
        }
    
}
