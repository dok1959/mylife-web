using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyLife
{
    public class AuthOptions
    {
        public const string ISSUER = "MyLifeWeb"; // издатель токена
        public const string AUDIENCE = "MyLifeWeb"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 120; // время жизни токена - 2 минуты
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
