// Request DTOs
export interface LoginDto {
  code: string;
  password: string;
  ipAddress: string;
}

export interface RegisterDto {
  fullName: string;
  email: string;
  password: string;

}

// Response DTOs
export interface AuthLoginResponse {
  accessToken: string;
  accessTokenExpires: string;
  refreshToken: string;
}
export interface AuthRegisterResponse {
  code: string;
}


export interface AuthStatus {
  authenticated: boolean;
}
