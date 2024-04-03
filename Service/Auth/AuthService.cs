using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MintCartWebApi.Common;
using MintCartWebApi.Data;
using MintCartWebApi.Helper;
using MintCartWebApi.LoggerService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MintCartWebApi.Service.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;

        public AuthService(ApplicationDbContext context, ILoggerManager logger
            , IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }


        public async Task<string> Authenticate(string userEmail, string password)
        {
            try
            {
                _logger.LogInfo("You are entering the authentication process.");

                var exUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userEmail);
                if (exUser == null)
                {
                    _logger.LogWarning($"User with email {userEmail} not found.");
                    return null;
                }

                var pepper = _configuration["AppSettings:pepper"];
                var iteration = _configuration["AppSettings:iteration"];
                var passwordHash = PasswordHasher.ComputeHash(password, exUser.PasswordSalt, pepper, Convert.ToInt32(iteration));
                if (exUser.PasswordHash != passwordHash)
                {
                    _logger.LogWarning($"Incorrect password for user {userEmail}.");
                    return null;
                }

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userEmail),
            new Claim(ClaimTypes.Name, exUser.UserId.ToString())
        };

                // Determine user role and add it to claims
                switch ((Enums.Roles)exUser.UserRole)
                {
                    case Enums.Roles.Admin:
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        break;
                    case Enums.Roles.SubAdmin:
                        claims.Add(new Claim(ClaimTypes.Role, "SubAdmin"));
                        break;
                    default:
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                        break;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var validToken =  tokenHandler.WriteToken(token);

                _context.Users.Update(exUser);
                await _context.SaveChangesAsync();
                _logger.LogInfo($"User successfully logged in with userEmail: {userEmail}");
                return validToken?.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during authentication: {ex.Message}");
                throw;
            }
        }

    }
}
