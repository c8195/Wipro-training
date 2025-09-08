using DoConnect.API.Data;
using DoConnect.API.Models;
using DoConnect.API.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoConnect.API.Services
{
    public class AuthServiceMinimal : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthServiceMinimal> _logger;

        public AuthServiceMinimal(
            UserManager<User> userManager,
            IConfiguration configuration,
            ILogger<AuthServiceMinimal> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation("Starting registration for user: {Username}", registerDto?.UserName ?? "NULL");

                if (registerDto == null)
                {
                    throw new ArgumentNullException(nameof(registerDto));
                }

                // Manual validation
                if (string.IsNullOrWhiteSpace(registerDto.FirstName))
                {
                    throw new ArgumentException("FirstName is required");
                }

                if (string.IsNullOrWhiteSpace(registerDto.LastName))
                {
                    throw new ArgumentException("LastName is required");
                }

                if (string.IsNullOrWhiteSpace(registerDto.Email))
                {
                    throw new ArgumentException("Email is required");
                }

                if (string.IsNullOrWhiteSpace(registerDto.UserName))
                {
                    throw new ArgumentException("UserName is required");
                }

                if (string.IsNullOrWhiteSpace(registerDto.Password))
                {
                    throw new ArgumentException("Password is required");
                }

                _logger.LogInformation("Validation passed, checking existing users");

                // Check existing users
                var existingEmail = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingEmail != null)
                {
                    throw new InvalidOperationException("Email already exists");
                }

                var existingUsername = await _userManager.FindByNameAsync(registerDto.UserName);
                if (existingUsername != null)
                {
                    throw new InvalidOperationException("Username already exists");
                }

                _logger.LogInformation("Creating new user");

                // Create user
                var user = new User
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    UserName = registerDto.UserName,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _logger.LogInformation("Calling UserManager.CreateAsync");

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("User creation failed: {Errors}", errors);
                    throw new InvalidOperationException($"Registration failed: {errors}");
                }

                _logger.LogInformation("User created successfully, adding role");

                await _userManager.AddToRoleAsync(user, "User");

                _logger.LogInformation("Generating JWT token");

                var token = GenerateJwtTokenMinimal(user);

                // Manual DTO creation (no AutoMapper)
                var userDto = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    UserName = user.UserName ?? string.Empty,
                    Roles = new List<string> { "User" }
                };

                return new AuthResponseDto
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddDays(7),
                    User = userDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RegisterAsync - Message: {Message}, StackTrace: {StackTrace}", 
                    ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Login attempt for user: {Username}", loginDto?.UserName ?? "NULL");

                if (loginDto == null)
                    throw new ArgumentNullException(nameof(loginDto));

                var user = await _userManager.FindByNameAsync(loginDto.UserName);
                if (user == null)
                {
                    throw new UnauthorizedAccessException("Invalid credentials");
                }

                var isValidPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isValidPassword)
                {
                    throw new UnauthorizedAccessException("Invalid credentials");
                }

                var token = GenerateJwtTokenMinimal(user);
                var roles = await _userManager.GetRolesAsync(user);

                var userDto = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    UserName = user.UserName ?? string.Empty,
                    Roles = roles.ToList()
                };

                return new AuthResponseDto
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddDays(7),
                    User = userDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LoginAsync");
                throw;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                Roles = roles.ToList()
            };
        }

        public async Task<bool> IsUserAdminAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;
            return await _userManager.IsInRoleAsync(user, "Admin");
        }

        private string GenerateJwtTokenMinimal(User user)
        {
            try
            {
                _logger.LogInformation("Generating JWT token for user: {UserId}", user.Id);

                var secretKey = _configuration["JwtSettings:SecretKey"];
                var issuer = _configuration["JwtSettings:Issuer"];
                var audience = _configuration["JwtSettings:Audience"];

                if (string.IsNullOrWhiteSpace(secretKey))
                {
                    _logger.LogError("JWT SecretKey is null or empty");
                    throw new InvalidOperationException("JWT SecretKey not configured");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature),
                    Issuer = issuer,
                    Audience = audience
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token");
                throw;
            }
        }
    }
}