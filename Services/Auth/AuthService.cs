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
        private readonly IUserRoleService _roleService; // ✅ NUEVO

        public AuthService(
            IUserRepository userRepo,
            ICustomPasswordService passwordService,
            IConfiguration configuration,
            IAuditService audit,
            IUserRoleService roleService // ✅ NUEVO
        )
        {
            _userRepo = userRepo;
            _passwordService = passwordService;
            _configuration = configuration;
            _audit = audit;
            _roleService = roleService;
        }

        public async Task<UserLoginResponseDTO?> LoginAsync(UserLoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
                return null;

            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null)
            {
                await _audit.InsertLogAsync(
                    null,
                    "LOGIN_FAIL",
                    "UserAccount",
                    null,
                    "Login failed: invalid credentials"
                );
                return null;
            }

            if (!user.IsActive)
            {
                await _audit.InsertLogAsync(
                    user.UserId,
                    "LOGIN_FAIL",
                    "UserAccount",
                    user.UserId,
                    "Login failed: account inactive"
                );
                return null;
            }

            if (!_passwordService.VerifyPasswordHash(
                request.Password,
                user.PasswordHash,
                user.PasswordSalt))
            {
                await _audit.InsertLogAsync(
                    user.UserId,
                    "LOGIN_FAIL",
                    "UserAccount",
                    user.UserId,
                    "Login failed: invalid credentials"
                );
                return null;
            }

            // ✅ OBTENER ROL POR ID
            var role = await _roleService.GetByIdAsync(user.RoleId);
            if (role == null || !role.IsActive)
            {
                return null;
            }

            var jwt = GenerateJwtToken(user, role);

            await _audit.InsertLogAsync(
                user.UserId,
                "LOGIN_SUCCESS",
                "UserAccount",
                user.UserId,
                "User logged in successfully"
            );

            return new UserLoginResponseDTO
            {
                Token = jwt.Token,
                ExpiresAt = jwt.ExpiresAt,
                User = new UserAccountDTO
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    IsActive = user.IsActive
                }
            };
        }

        // ✅ AHORA RECIBE EL ROL
        private (string Token, DateTime ExpiresAt) GenerateJwtToken(UserAccount user, RoleDTO role)
        {
            var jwtSection = _configuration.GetSection("Jwt");

            var issuer = jwtSection.GetValue<string>("Issuer")
                ?? throw new InvalidOperationException("Jwt:Issuer is not configured");

            var audience = jwtSection.GetValue<string>("Audience")
                ?? throw new InvalidOperationException("Jwt:Audience is not configured");

            var secret = jwtSection.GetValue<string>("Secret")
                ?? throw new InvalidOperationException("Jwt:Secret is not configured");

            var expiresMinutes = jwtSection.GetValue<int>("ExpiresMinutes", 60);

            if (secret.Length < 32)
                throw new InvalidOperationException("JWT Secret must be at least 32 characters");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),

                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),

                // ✅ CLAVE: NOMBRE DEL ROL
                new Claim(ClaimTypes.Role, role.Name)
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
