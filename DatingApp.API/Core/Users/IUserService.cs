using System.Threading.Tasks;
using DatingApp.API.Core.Base;
using DatingApp.API.Core.Users.DTO;

namespace DatingApp.API.Core.Users
{
    public interface IUserService
    {
        Task<BaseReturnModel<bool>> Register(UserRegisterInputDTO userInfor);
        Task<BaseReturnModel<AccessToken>> Login(LoginDTO loginDTO);
    }

}