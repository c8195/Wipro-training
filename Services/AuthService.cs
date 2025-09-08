using AutoMapper;
using DoConnect.API.Data;
using DoConnect.API.Models;
using DoConnect.API.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoConnect.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            IMapper mapper,
            ApplicationDbContext context,
            ILogger<AuthService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                if (loginDto == null)
                    throw new ArgumentNullException(nameof(loginDto));

                if (string.IsNullOrWhiteSpace(loginDto.UserName))
                    throw new ArgumentException("Username cannot be null or empty", nameof(loginDto.UserName));

                if (string.IsNullOrWhiteSpace(loginDto.Password))
                    throw new ArgumentException("Password cannot be null or empty", nameof(loginDto.Password));

                var user = await _userManager.FindByNameAsync(loginDto.UserName);
                if (user == null)
                {
                    _logger.LogWarning("Login attempt with invalid username: {Username}", loginDto.UserName);
                    throw new UnauthorizedAccessException("Invalid credentials");
                }

                var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!passwordValid)
                {
                    _logger.LogWarning("Login attempt with invalid password for user: {Username}", loginDto.UserName);
                    throw new UnauthorizedAccessException("Invalid credentials");
                }

                if (!user.IsActive)
                {
                    _logger.LogWarning("Login attempt for deactivated user: {Username}", loginDto.UserName);
                    throw new UnauthorizedAccessException("Account is deactivated");
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user, userRoles);

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = userRoles?.ToList() ?? new List<string>();

                _logger.LogInformation("User {Username} logged in successfully", loginDto.UserName);

                return new AuthResponseDto
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddDays(7),
                    User = userDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for username: {Username}", loginDto?.UserName);
                throw;
            }
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                if (registerDto == null)
                    throw new ArgumentNullException(nameof(registerDto));

                // Validate all required fields
                if (string.IsNullOrWhiteSpace(registerDto.FirstName))
                    throw new ArgumentException("First name cannot be null or empty", nameof(registerDto.FirstName));

                if (string.IsNullOrWhiteSpace(registerDto.LastName))
                    throw new ArgumentException("Last name cannot be null or empty", nameof(registerDto.LastName));

                if (string.IsNullOrWhiteSpace(registerDto.Email))
                    throw new ArgumentException("Email cannot be null or empty", nameof(registerDto.Email));

                if (string.IsNullOrWhiteSpace(registerDto.UserName))
                    throw new ArgumentException("Username cannot be null or empty", nameof(registerDto.UserName));

                if (string.IsNullOrWhiteSpace(registerDto.Password))
                    throw new ArgumentException("Password cannot be null or empty", nameof(registerDto.Password));

                // Check if email already exists
                var existingEmailUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingEmailUser != null)
                {
                    _logger.LogWarning("Registration attempt with existing email: {Email}", registerDto.Email);
                    throw new InvalidOperationException("Email already exists");
                }

                // Check if username already exists
                var existingUserNameUser = await _userManager.FindByNameAsync(registerDto.UserName);
                if (existingUserNameUser != null)
                {
                    _logger.LogWarning("Registration attempt with existing username: {Username}", registerDto.UserName);
                    throw new InvalidOperationException("Username already exists");
                }

                var user = new User
                {
                    FirstName = registerDto.FirstName.Trim(),
                    LastName = registerDto.LastName.Trim(),
                    Email = registerDto.Email.Trim().ToLowerInvariant(),
                    UserName = registerDto.UserName.Trim(),
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("User registration failed for {Username}: {Errors}", registerDto.UserName, errors);
                    throw new InvalidOperationException($"Registration failed: {errors}");
                }

                // Assign User role by default
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    _logger.LogWarning("Failed to assign User role to {Username}", registerDto.UserName);
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user, userRoles);

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = userRoles?.ToList() ?? new List<string>();

                _logger.LogInformation("User {Username} registered successfully", registerDto.UserName);

                return new AuthResponseDto
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddDays(7),
                    User = userDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for username: {Username}", registerDto?.UserName);
                throw;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {UserId}", userId);
                    throw new KeyNotFoundException("User not found");
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = userRoles?.ToList() ?? new List<string>();

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> IsUserAdminAsync(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {UserId} for admin check", userId);
                    return false;
                }

                return await _userManager.IsInRoleAsync(user, "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking admin status for user ID: {UserId}", userId);
                return false;
            }
        }

        private string GenerateJwtToken(User user, IList<string> roles)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                var secretKey = _configuration["JwtSettings:SecretKey"];
                if (string.IsNullOrWhiteSpace(secretKey))
                    throw new InvalidOperationException("JWT SecretKey is not configured");

                var issuer = _configuration["JwtSettings:Issuer"];
                if (string.IsNullOrWhiteSpace(issuer))
                    throw new InvalidOperationException("JWT Issuer is not configured");

                var audience = _configuration["JwtSettings:Audience"];
                if (string.IsNullOrWhiteSpace(audience))
                    throw new InvalidOperationException("JWT Audience is not configured");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new(ClaimTypes.Email, user.Email ?? string.Empty),
                    new("firstName", user.FirstName ?? string.Empty),
                    new("lastName", user.LastName ?? string.Empty)
                };

               foreach (var role in roles)
                    {
                    if (!string.IsNullOrWhiteSpace(role))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                        _logger.LogInformation("Added role claim:{Role}", role);
                        }
                    }
           

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
                _logger.LogError(ex, "Error generating JWT token for user: {Username}", user?.UserName);
                throw;
            }
        }
    }
}