using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomCADSolutions.API.Helpers
{
    public static class Utilities
    {

        public static async Task<string> GenerateJwtTokenAsync(this UserManager<AppUser> userManager, AppUser user, IConfiguration config)
        {
            byte[] key = Encoding.ASCII.GetBytes(config["JwtSettings:SecretKey"]!);

            JwtSecurityTokenHandler tokenHandler = new();

            SecurityToken token = tokenHandler.CreateToken(new()
            {
                Subject = new(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Email, user.Email!),
                    new(ClaimTypes.Name, user.UserName!),
                    new(ClaimTypes.Role, (await userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = config["JwtSettings:Audience"],
                Issuer = config["JwtSettings:Issuer"],
            });
            return tokenHandler.WriteToken(token);
        }
    }
}
