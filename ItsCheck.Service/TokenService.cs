using AutoMapper;
using ItsCheck.Domain.Identity;
using ItsCheck.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ItsCheck.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config,
                            UserManager<User> userManager,
                            IMapper mapper)
        {
            _config = config;
            _userManager = userManager;
            _mapper = mapper;
            _key = GetSecurityKey(config["TokenKey"]!);
        }

        private SymmetricSecurityKey GetSecurityKey(string key)
        {
            if (string.IsNullOrEmpty(key) || key.Length < 64)
            {
                byte[] keyBytes = new byte[64];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(keyBytes);
                }
                key = Convert.ToBase64String(keyBytes);
                _config["TokenKey"] = key;
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public async Task<string> CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            claims.AddRange(user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role.Role.Name!)));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
