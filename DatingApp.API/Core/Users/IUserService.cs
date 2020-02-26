using System.Threading.Tasks;
using DatingApp.API.Core.Base;

namespace DatingApp.API.Core.Users
{
    public interface IUserService
    {
        Task<BaseReturnModel<bool>> Register(UserRegisterInputDTO userInfor);
        Task<BaseReturnModel<AccessToken>> Login(string userName, string passWord);
    }

}