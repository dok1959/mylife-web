using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MyLife.Services.TokenValidators
{
    public class RefreshTokenValidator
    {
        private IConfiguration _configuration;

        public RefreshTokenValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Validate(string refreshToken)
        {
            var refreshTokenConfig = _configuration.GetSection("Authentication");
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshTokenConfig["RefreshTokenSecret"])),
                ValidIssuer = refreshTokenConfig["Issuer"],
                ValidAudience = refreshTokenConfig["Audience"],
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
