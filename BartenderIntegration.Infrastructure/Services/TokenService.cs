using BartenderIntegration.Infrastructure.Identity;
using BartenderIntegration.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BartenderIntegration.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtHandler _jwt;

        public TokenService(UserManager<AppUser> userManager, IOptions<JwtHandler> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        public async Task<AuthorizedModel> GenerateTokenAsync(AppUser user)
        {
            var signinKey = Credentials();
            var claims = await GetClaimsAsync(user);
            var token = new JwtSecurityToken(_jwt.Issuer, _jwt.Audience, claims, expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes), signingCredentials: signinKey);
            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
            return new AuthorizedModel
            {
                AccessToken = tokenHandler,
                ExpiresIn = token.ValidTo
            };
        }

        private SigningCredentials Credentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaimsAsync(AppUser user)
        {
            var userRole = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Iss,_jwt.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
            };
            foreach (var role in userRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }

    public interface ITokenService
    {
        Task<AuthorizedModel> GenerateTokenAsync(AppUser user);
    }
}
