using System.Security.Cryptography;
using System.Text;

namespace CTG_Api.Utilidades
{
    public class SeguridadContraseña
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerificarPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
