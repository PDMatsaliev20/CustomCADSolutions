using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static CustomCADs.Domain.DataConstants.UserConstants;

namespace CustomCADs.API.Helpers;

public static class JwtHelper
{
    public static JwtSecurityToken GenerateAccessToken(this IConfiguration config, string id, string username, string role)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, role),
            new(ClaimTypes.NameIdentifier, id),
        ];

        string? key = config["JwtSettings:SecretKey"];
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        SymmetricSecurityKey security = new(Encoding.UTF8.GetBytes(key));
        string algorithm = SecurityAlgorithms.HmacSha256;

        JwtSecurityToken token = new(
            issuer: config["JwtSettings:Issuer"],
            audience: config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: new SigningCredentials(security, algorithm)
        );

        return token;
    }

    public static string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        RandomNumberGenerator.Fill(randomNumber);            
        return Base64UrlEncoder.Encode(randomNumber);
    }

    public static async Task<(string value, DateTime end)> RenewRefreshToken(this IUserService userService, UserModel user)
    {
        string newRT = GenerateRefreshToken();
        DateTime newEndDate = DateTime.UtcNow.AddDays(RefreshTokenDaysLimit);

        user.RefreshToken = newRT;
        user.RefreshTokenEndDate = newEndDate;
        await userService.EditAsync(user.UserName, user).ConfigureAwait(false);

        return (newRT, newEndDate);
    }
}
