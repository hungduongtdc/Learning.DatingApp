using System.Linq;

namespace DatingApp.API.Core.HashingHelper
{
    public class HashingHelper : IHashingHelper
    {

        public void GeneratePassWordHash(string password, out byte[] passWordHash, out byte[] passWordSalt)
        {
            using (var hcmac = new System.Security.Cryptography.HMACSHA512())
            {
                passWordSalt = hcmac.Key;
                passWordHash = hcmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool CheckPassword(string password, byte[] passWordHash, byte[] passWordSalt)
        {
            using (var hcmac = new System.Security.Cryptography.HMACSHA512(passWordSalt))
            {
                var passWordHashed = hcmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return passWordHash.SequenceEqual(passWordHashed);
            }
        }
    }
}