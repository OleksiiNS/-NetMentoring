using System.Security.Cryptography;

namespace GeneratePassword
{
    internal class Generator
    {
        private const int Iterate = 10000;
        private readonly byte[] _hashBytes = new byte[36];
        
        public string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            using var rfc = new Rfc2898DeriveBytes(passwordText, salt, Iterate);
            var hash = rfc.GetBytes(20);

            Array.Copy(salt, 0, _hashBytes, 0, 16);
            Array.Copy(hash, 0, _hashBytes, 16, 20);
        
            return Convert.ToBase64String(_hashBytes);
        }
    }
}
