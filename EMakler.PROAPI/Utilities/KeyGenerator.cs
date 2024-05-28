using System.Security.Cryptography;

namespace EMakler.PROAPI.Utilities
{
    public static class KeyGenerator
    {
        public static string GenerateSecretKey(int keySize)
        {
            var randomNumberGenerator = new RNGCryptoServiceProvider();
            var randomNumber = new byte[keySize / 8];
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
