using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace CustomCADSolutions.API.Helpers
{
    public static class JwtExtensions
    {
        public static bool IsTokenValid(string jwt, string secretKey, string issuer, string audience)
        {
            try
            {
                TokenValidationParameters validationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey))
                };

                new JwtSecurityTokenHandler().ValidateToken(jwt, validationParameters, out _);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static byte[] GetJwtSecretKey(this IConfiguration config)
        {
            string? secretKey = config["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                secretKey = 32.GenerateSecretKey();
                config["JwtSettings:SecretKey"] = secretKey;
            }
            byte[] keyBytes = Encoding.ASCII.GetBytes(secretKey);

            return keyBytes;
        }

        private static string GenerateSecretKey(this int keyLengthInBytes)
        {
            byte[] keyBytes = new byte[keyLengthInBytes];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(keyBytes);

            string secretKey = Convert.ToBase64String(keyBytes);
            return secretKey;
        }
    }
}
