using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace WriteTogether.Features.Authorization
{
    public class AuthorizationService
    {
        private readonly AuthorizationRepository _repo;
        private readonly IConfiguration _config;
        public AuthorizationService(IConfiguration config, AuthorizationRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        public async Task<string?> Login(AuthorizationModel request)
        {
            var user = await _repo.GetUserByEmail(request.Email);

            if (user == null)
                return null;

            if (user.Password != request.Password)
                return null;

            return GenerateTokenJwt(user.UserId, user.Email, user.UserName);
        }

        public async Task<string?> FetchUsers(AuthorizationModel request)
        {
            var user = await _repo.GetUserByEmail(request.Email);

            if (user == null)
                return null;

            if (user.UserName == null)
                return null;

            if (user.Password != request.Password)
                return null;

            return user.UserName;
        }

        public string GenerateTokenJwt(int userId, string email, string userName)
        {
            var secretKey = _config["Jwt:Key"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, userName),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}
