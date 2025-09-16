export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  roles: string[];
}

export interface LoginRequest {
  userName: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  password: string;
  confirmPassword: string;
}

export interface AuthResponse {
  token: string;
  expiration: string;
  user: User;
}