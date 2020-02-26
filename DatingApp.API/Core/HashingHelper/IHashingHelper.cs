namespace DatingApp.API.Core.HashingHelper
{
    public interface IHashingHelper
    {
        void GeneratePassWordHash(string password, out byte[] passWordHash, out byte[] passWordSalt);
        bool CheckPassword(string password, byte[] passWordHash, byte[] passWordSalt);
    }
}