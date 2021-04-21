using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyLife
{
    public class AuthenticationConfiguration
    {
        public string TokenSecret { get; set; }
        public double TokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenSecret));
        }
    }
}
