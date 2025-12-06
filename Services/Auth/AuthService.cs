using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StoreApi.Interface.Audit;
using StoreApi.Interface.User;
using StoreApi.Models;
using StoreApi.ModelsDTO.User;
using StoreApi.Repositorys.User;
using StoreApi.Tools;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace StoreApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly ICustomPasswordService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly IAuditService _audit;

        public AuthService(IUserRepository userRepo, ICustomPasswordService passwordService, IConfiguration configuration, IAuditService audit)
        {
            _userRepo = userRepo;
            _passwordService = passwordService;
            _configuration = configuration;
            _audit = audit;
        }

        public async Task<UserLoginResponseDTO?> LoginAsync(UserLoginRequestDTO request)
        {
            // 1) Basic validation
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null)
            {
                // log failed login attempt (no user)
                await _audit.InsertLogAsync(null, "LOGIN_FAIL", "UserAccount", null, $"Login failed: email '{request.Email}' not found");
                return null;
            }

            if (!user.IsActive)
            {
                await _audit.InsertLogAsync(user.UserId, "LOGIN_FAIL", "UserAccount", user.UserId, "Login failed: account inactive");
                return null;
            }

            // Verify password with the provided salt/hash
            var isValid = _passwordService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!isValid)
            {
                await _audit.InsertLogAsync(user.UserId, "LOGIN_FAIL", "UserAccount", user.UserId, "Login failed: invalid password");
                return null;
            }

            // Generate JWT
            var token = GenerateJwtToken(user);

            // Insert successful login log
            await _audit.InsertLogAsync(user.UserId, "LOGIN_SUCCESS", "UserAccount", user.UserId, "User logged in successfully.");

            // Map to DTO
            var userDto = new UserAccountDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CardId = user.CardId,
                IsActive = user.IsActive,
                RoleId = user.RoleId,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt == DateTime.MinValue ? null : (DateTime?)user.UpdatedAt
            };

            return new UserLoginResponseDTO
            {
                Token = token.Token,
                User = userDto,
                ExpiresAt = token.ExpiresAt
            };
        }

        private (string Token, DateTime ExpiresAt) GenerateJwtToken(UserAccount user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var secret = jwtSection.GetValue<string>("Secret");
            var expiresMinutes = jwtSection.GetValue<int>("ExpiresMinutes", 60);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);

            // Claims: subject, name, email, role, custom claims as required
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, user.RoleId.ToString()) // consider mapping role id -> role name
        };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expires);
        }
    }
}