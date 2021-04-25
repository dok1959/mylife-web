using Microsoft.Extensions.Configuration;
using MyLife.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace MyLife.Services.TokenGenerators
{
    public class AccessTokenGenerator
    {
        private IConfiguration _config;
        private TokenGenerator _tokenGenerator;

        public AccessTokenGenerator(IConfiguration config, TokenGenerator tokenGenerator)
        {
            _config = config;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken(User user)
        {
            var accessTokenConfig = _config.GetSection("Authentication");

            List<Claim> claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var generatedToken = _tokenGenerator.GenerateToken(
                accessTokenConfig["AccessTokenSecret"],
                accessTokenConfig["Issuer"],
                accessTokenConfig["Audience"],
                double.Parse(accessTokenConfig["AccessTokenExpirationMinutes"]),
                claims
                );

            return generatedToken;
        }
    }
}
