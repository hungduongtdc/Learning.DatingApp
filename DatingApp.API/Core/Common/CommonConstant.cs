using System.Runtime.Serialization;

namespace DatingApp.API.Core.Common
{
    public static class CommonConstant
    {
        public const string ERROR_UserNameIsAlreadyTaken = "This user name is already taken. Please choose another name.";
        public const string ERROR_UserNotFound = "Username not found!";
        public const string ERROR_InvalidCredential = "Invalid credential";
    }
}