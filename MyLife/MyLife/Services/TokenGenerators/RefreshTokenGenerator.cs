using Microsoft.Extensions.Configuration;

namespace MyLife.Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private IConfiguration _config;
        private TokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(IConfiguration config, TokenGenerator tokenGenerator)
        {
            _config = config;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken()
        {
            var refreshTokenConfig = _config.GetSection("Authentication");

            var generatedToken = _tokenGenerator.GenerateToken(
                refreshTokenConfig["RefreshTokenSecret"],
                refreshTokenConfig["Issuer"],
                refreshTokenConfig["Audience"],
                double.Parse(refreshTokenConfig["RefreshTokenExpirationMinutes"])
                );

            return generatedToken;
        }
    }
}